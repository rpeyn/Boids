using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    Boid[] boids;
    Vector2 myBoidPos;
    Vector2 otherBoidPos;
    float distance, threshold = 5f, avoidThreshold = 2.5f;
    int neighborhoodSize = 0;
    Vector2 avgNeighborhoodDir = Vector2.zero;
    Vector2 avgNeighborhoodPos = Vector2.zero;
    Vector2 avgAvoidNeighborhoodPos = Vector2.zero;
    Vector2 offsetToNeighborhoodCenter = Vector2.zero;
    Vector2 offsetAvoidCenter = Vector2.zero;
    Vector2 acceleration = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        boids = FindObjectsOfType<Boid>();
        Debug.Log(boids.Length);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < boids.Length; i++)
        {
            avgNeighborhoodDir = Vector2.zero;
            avgNeighborhoodPos = Vector2.zero;
            neighborhoodSize = 0;
            acceleration = Vector2.zero;

            myBoidPos = boids[i].transform.position;
            for(int j = 0; j < boids.Length; j++)
            {
                if( i != j)
                {
                    otherBoidPos = boids[j].transform.position;
                    distance = Vector2.Distance(myBoidPos, otherBoidPos);
                    if( distance < threshold)
                    {
                        neighborhoodSize++;
                        avgNeighborhoodDir += boids[j].m_Rigidbody.velocity.normalized;
                        avgNeighborhoodPos += new Vector2(otherBoidPos.x, otherBoidPos.y);
                        if ( distance < avoidThreshold)
                        {
                            offsetAvoidCenter = TurnV3toV2(otherBoidPos) - TurnV3toV2(myBoidPos);
                            acceleration -= offsetAvoidCenter / Vector2.SqrMagnitude(offsetAvoidCenter);
                        }
                    }

                }
            }

            if( neighborhoodSize > 0 )
            {
                avgNeighborhoodDir/= neighborhoodSize;

                avgNeighborhoodPos/= neighborhoodSize;
                offsetToNeighborhoodCenter = avgNeighborhoodPos - new Vector2(myBoidPos.x, myBoidPos.y);
            }

            acceleration += avgNeighborhoodDir + offsetToNeighborhoodCenter;
            boids[i].m_Rigidbody.velocity = Vector2.ClampMagnitude(boids[i].m_Rigidbody.velocity + acceleration * Time.deltaTime, boids[i].m_Speed);


        }
    }

    Vector2 TurnV3toV2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
}
