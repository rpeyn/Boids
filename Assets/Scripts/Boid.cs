using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boid : MonoBehaviour
{
    public Rigidbody2D boidRigidbody;
    public static float maxSpeed = 5f;

    //vars for obstacle detection with raycast
    float viewingRadius = 10f;
    float raycastCircleRadius = 2f;

    //the angle by which we rotate the boid when looking for a direction to steer towards
    float rotationAngle = Mathf.Deg2Rad * 20f;

    //vars for deciding if steering to the left or right
    int steeringDirection = 0;
    float offsetHitClock = 0, offsetHitCounterClock = 0;

    void Start()
    {
        //the boid starts moving in a random direction with the maximum speed
        boidRigidbody.velocity = Random.insideUnitCircle * maxSpeed;

        //picks a random bird character (from the 3 options that are unactive children to the boid prefab)
        GameObject bird = this.transform.GetChild(Random.Range(0, 3)).gameObject;
        bird.SetActive(true);

        //setting up the max speed UI slider
        Slider maxSpeedSlider = GameObject.Find("Max Speed Slider").GetComponent<Slider>();
        TMP_Text maxSpeedText= GameObject.Find("Max Speed Text").GetComponent<TMP_Text>();
        SetSlider(maxSpeedSlider, maxSpeedText, maxSpeed);
    }


    /*
    this function is called in Boids Manager 
    manages detection and avoidance of obstacles
    adds the acceleration to the velocity
    */
    public void UpdateAcceleration(Vector2 acceleration)
    {
        //rotates the boid in the direction of movement
        if (boidRigidbody.velocity.normalized != Vector2.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, boidRigidbody.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, boidRigidbody.velocity.magnitude * 25f * Time.deltaTime);
        }

        //detects obstacles in the viewing radius
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, raycastCircleRadius,boidRigidbody.velocity.normalized, viewingRadius);

        //if an object is detected and has an "obstacle" tag
        if (hit.collider != null && hit.transform.tag == "Obstacle")
        {
                //detects obstacles to the left and right / clockwize and counter clockwise  
                RaycastHit2D hitClock = Physics2D.CircleCast(transform.position, raycastCircleRadius,Rotate(boidRigidbody.velocity.normalized, -1), viewingRadius);
                RaycastHit2D hitCounterClock = Physics2D.CircleCast(transform.position, raycastCircleRadius,Rotate(boidRigidbody.velocity.normalized, 1), viewingRadius);

                //measures offset to the obstacle clockwise
                if (hitClock.collider != null)
                {
                    offsetHitClock = (transform.position - hitClock.transform.position).magnitude;
                }
                else offsetHitClock = float.PositiveInfinity;
                
                //measures offset to the obstacle counter clockwise
                if(hitCounterClock.collider != null)
                {
                    offsetHitCounterClock = (transform.position - hitCounterClock.transform.position).magnitude;
                }else offsetHitCounterClock = float.PositiveInfinity;

                //compares the two offsets and gives a steering direction (clockwise or counter~) towards the one that's further away
                if (offsetHitCounterClock > offsetHitClock)
                {
                    steeringDirection = 1;
                }
                else { steeringDirection = -1; }

            //scales the new steering vector to the maximum speed and a weight and adds it to the acceleration
            acceleration += FindSteeringDirection(boidRigidbody.velocity.normalized, steeringDirection) * maxSpeed * 5f;
        }
        

        //adds the acceleration to the boid's velocity, clamping it to the maximum speed
        boidRigidbody.velocity = Vector2.ClampMagnitude(boidRigidbody.velocity + acceleration * Time.deltaTime, maxSpeed);
    }


    //function that finds a direction without obstacles
    //takes the current direction and the direction to start rotating towards (clockwise or counter clockwise / -1 or 1)
    private Vector2 FindSteeringDirection(Vector2 currentDirection, int dirRotation)
    {
        //while there is an obstacle in the given direction
        while (CheckForObstacle(currentDirection))
        {
            //rotate the direction once
            currentDirection = Rotate(currentDirection, dirRotation);
            
            //if the function has made a full circle (checked every direction and found none without an obstacle)
            if(currentDirection == boidRigidbody.velocity.normalized)
            {
                //don't change the boid's direction
                return currentDirection;
            }
        }
        //return the first found direction without an obstacle on it
        return currentDirection;
    }

    //function that takes a vector and the sign of the rotation (-1 for clockwise and +1 for counter clockwise)
    //and returns the same vector rotated by the given rotation angle once
    private Vector2 Rotate(Vector2 v, int sign)
    {
        return new Vector2(
            v.x * Mathf.Cos(sign * rotationAngle) - v.y * Mathf.Sin(sign * rotationAngle),
            v.x * Mathf.Sin(sign * rotationAngle) + v.y * Mathf.Cos(sign * rotationAngle)
        );
    }

    //function that checks if there is an obstacle in a given direction (true / false)
    private bool CheckForObstacle(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, raycastCircleRadius, direction, viewingRadius);
        return (hit.collider != null && hit.transform.tag == "Obstacle");
    }



    //-------- UI --------------
    //function to set a slider's value and value text
    public static void SetSlider(Slider slider, TMP_Text number,float value)
    {
        slider.value = value;
        number.text = value.ToString();
    }
}
