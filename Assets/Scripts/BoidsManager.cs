//The boids manager loops through every boid in the scene and updates its velocity
//based on Alignment, Cohesion, and Avoidance forces

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoidsManager : MonoBehaviour
{
    Boid[] boids;
    Vector2 myBoidPos, otherBoidPos;

    float distanceOtherBoid, neighborThreshold = 7f, avoidThreshold = 4.5f;
    int neighborhoodSize = 0;

    //the three forces: Alignment, Cohesion, and Avoidance
    Vector2 avgNeighborhoodDir, offsetToNeighborhoodCenter = Vector2.zero, offsetAvoidance = Vector2.zero;
    Vector2 avgNeighborhoodPos, acceleration;

    //weights of the three forces
    public static int weightAvoid = 5, weightAlign = 2, weightCohes = 1;

    //UI elements for changing the weights
    public Slider sliderWeightAvoid, sliderWeightAlign, sliderWeightCohes;
    public TMP_Text textSliderAvoid, textSliderAlign, textSliderCohes;

    void Start()
    {
        boids = FindObjectsOfType<Boid>();

        //setting up the weights sliders
        Boid.SetSlider(sliderWeightAvoid, textSliderAvoid, weightAvoid);
        Boid.SetSlider(sliderWeightAlign, textSliderAlign, weightAlign);
        Boid.SetSlider(sliderWeightCohes, textSliderCohes, weightCohes);
    }

    void Update()
    {
        //loop through every boid's perspective
        for(int i = 0; i < boids.Length; i++)
        {
            //reset all values
            avgNeighborhoodDir = avgNeighborhoodPos = acceleration = Vector2.zero;
            neighborhoodSize = 0;

            //get the position of current boid
            myBoidPos = boids[i].transform.position;

            //loop through all other boids
            for(int j = 0; j < boids.Length; j++)
            {
                if( i != j)
                {
                    //get the position of the other boid
                    otherBoidPos = boids[j].transform.position;

                    //measure distance between my boid and the other boid
                    distanceOtherBoid = Vector2.Distance(myBoidPos, otherBoidPos);

                    //check if the other boid is close enough to be a "neighbor"
                    if( distanceOtherBoid < neighborThreshold)
                    {
                        neighborhoodSize++;
                        /*
                        add the direction of the other boid to the average direction of the neighborhood
                        direction of the boid is the normalized velocity vector
                        the velocity has a direction and a length/speed - when normalized the length is 1, so it only represents the direction
                        */
                        avgNeighborhoodDir += boids[j].boidRigidbody.velocity.normalized;

                        //add the position of the other boid to the average position of the neighborhood
                        avgNeighborhoodPos += TurnV3toV2(otherBoidPos);

                        //check if the other boid is too close and needs to be avoided
                        if ( distanceOtherBoid < avoidThreshold)
                        {
                            //measure offset between my boid and the other boid
                            //offset is the vector between the two (both direction and length)
                            offsetAvoidance = TurnV3toV2(otherBoidPos) - TurnV3toV2(myBoidPos);

                            //AVOIDANCE FORCE ADDED
                            /*
                            add the avoidance force to the acceleration
                            offsetAvoidance / offsetAvoidance's length^2 because the effect(length) needs to be reversed
                            the smaller the offset the bigger the avoidance force and vice-versa
                            inspired by Sebastian Lague's Boids project https://youtu.be/bqtqltqcQhw?si=EDA9WktGs2Vs4Nr7
                            */
                            acceleration -= offsetAvoidance * weightAvoid / Vector2.SqrMagnitude(offsetAvoidance);
                        }
                    }

                }
            }

            //check if there are neighbors
            if( neighborhoodSize > 0 )
            {
                //find the average of all neighbors' directions
                avgNeighborhoodDir/= neighborhoodSize;

                //find the average of all neighbors' positions
                //the center of the neighborhood
                avgNeighborhoodPos/= neighborhoodSize;

                //offset from my boid to the center of the neighborhood
                offsetToNeighborhoodCenter = avgNeighborhoodPos - TurnV3toV2(myBoidPos);
            }

            //ALIGNMENT AND COHESION FORCES ADDED
            //add the neighborhood's direction and the offset from the neighborhood's center to my boid's acceleration
            acceleration += avgNeighborhoodDir * weightAlign + offsetToNeighborhoodCenter * weightCohes;

            //a function from the boid that adds obstacle avoidance to the acceleration
            //and adds the acceleration to the velocity
            boids[i].UpdateAcceleration(acceleration);
        }
    }

    //turns Vector3 into Vector2
    public Vector2 TurnV3toV2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }


    /*----------UI--------------
    functions for updating the weights from the weights sliders
    the 3 are exactly the same, just different variables
    */
    public void UpdateWeightAvoid()
    {
        weightAvoid = Mathf.RoundToInt(sliderWeightAvoid.value);
        textSliderAvoid.text = weightAvoid.ToString();
    }

    public void UpdateWeightAlign()
    {
        weightAlign = Mathf.RoundToInt(sliderWeightAlign.value);
        textSliderAlign.text = weightAlign.ToString();
    }
    
    public void UpdateWeightCohes()
    {
        weightCohes = Mathf.RoundToInt(sliderWeightCohes.value);
        textSliderCohes.text = weightCohes.ToString();
    }
}
