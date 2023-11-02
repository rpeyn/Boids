using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody2D boidRigidbody;
    public float maxSpeed = 2.0f;

    void Start()
    {
        //the boid starts moving in a random direction with the maximum speed
        boidRigidbody.velocity = Random.insideUnitCircle * maxSpeed;
    }
}
