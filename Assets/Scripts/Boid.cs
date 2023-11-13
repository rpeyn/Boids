using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody2D boidRigidbody;
    public float maxSpeed;
    float viewingRadius = 10f;
    float raycastCircleRadius = 2f;
    float rotationAngle = Mathf.Deg2Rad * 20f;

    int steeringDirection = 0;

    //bool isTurning = false;

    public GameObject velocityLine;

    float offsetHitClock = 0, offsetHitCounterClock = 0;

    void Start()
    {
        //the boid starts moving in a random direction with the maximum speed
        boidRigidbody.velocity = Random.insideUnitCircle * maxSpeed;
        //GameObject[] birds = GetComponentsInChildren<>();
        //birds[Random.Range(0, birds.Length)].SetActive(true);
        GameObject bird = this.transform.GetChild(Random.Range(0, 3)).gameObject;
        bird.SetActive(true);
    }

    public void UpdateAcceleration(Vector2 acceleration)
    {
        if (boidRigidbody.velocity.normalized != Vector2.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, boidRigidbody.velocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, boidRigidbody.velocity.magnitude * 25f * Time.deltaTime);
        }

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, raycastCircleRadius,boidRigidbody.velocity.normalized, viewingRadius);

        // If it hits something...
        if (hit.collider != null && hit.transform.tag == "Obstacle")
        {
                RaycastHit2D hitClock = Physics2D.CircleCast(transform.position, raycastCircleRadius,Rotate(boidRigidbody.velocity.normalized, -1), viewingRadius);
                RaycastHit2D hitCounterClock = Physics2D.CircleCast(transform.position, raycastCircleRadius,Rotate(boidRigidbody.velocity.normalized, 1), viewingRadius);

                if (hitClock.collider != null)
                {
                    offsetHitClock = (transform.position - hitClock.transform.position).magnitude;
                }
                else offsetHitClock = float.PositiveInfinity;
                
                if(hitCounterClock.collider != null)
                {
                    offsetHitCounterClock = (transform.position - hitCounterClock.transform.position).magnitude;
                }else offsetHitCounterClock = float.PositiveInfinity;

                if (offsetHitCounterClock > offsetHitClock)
                {
                    steeringDirection = 1;
                }
                else { steeringDirection = -1; }

            acceleration += FindSteeringDirection(boidRigidbody.velocity.normalized, steeringDirection)* 20f;
        }
        
        boidRigidbody.velocity = Vector2.ClampMagnitude(boidRigidbody.velocity + acceleration * Time.deltaTime, maxSpeed);
    }

    private Vector2 FindSteeringDirection(Vector2 currentDirection, int dirRotation)
    {
        int timesInWhile = 0;
        while (CheckForObstacle(currentDirection))
        {
            timesInWhile++;
            currentDirection = Rotate(currentDirection, dirRotation);
            if(currentDirection == boidRigidbody.velocity.normalized)
            {
                return currentDirection;
            }
        }
        return currentDirection;
    }

    private Vector2 Rotate(Vector2 v, int sign)
    {
        return new Vector2(
            v.x * Mathf.Cos(sign * rotationAngle) - v.y * Mathf.Sin(sign * rotationAngle),
            v.x * Mathf.Sin(sign * rotationAngle) + v.y * Mathf.Cos(sign * rotationAngle)
        );
    }


    private bool CheckForObstacle(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, raycastCircleRadius,direction, viewingRadius);
        return (hit.collider != null && hit.transform.tag == "Obstacle");
    }
}
