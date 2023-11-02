using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoids : MonoBehaviour
{
    public int numberBoids = 20;
    public GameObject boid;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0; i <numberBoids; i++)
        {
            GameObject boid_i = Instantiate(boid);
            boid_i.transform.position = Random.insideUnitCircle * 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
