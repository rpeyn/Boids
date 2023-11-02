using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoids : MonoBehaviour
{
    public int numberBoids = 20;
    public GameObject boid;
    public int spawnAreaBoids = 10;

    //on awake, spawn the boids on random positions in the scene
    void Awake()
    {
        for(int i=0; i <numberBoids; i++)
        {
            GameObject boid_i = Instantiate(boid);
            boid_i.transform.position = Random.insideUnitCircle * spawnAreaBoids;
        }
    }
}
