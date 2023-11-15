using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpawnerBoids : MonoBehaviour
{
    public static int numberBoids = 10;
    public GameObject boid;
    public int spawnAreaBoids = 10;

    //for changing number of boids
    public Slider sliderBoidsNumber;
    public TMP_Text boidsNumberText;


    //on awake, spawn the boids on random positions in the scene
    void Awake()
    {
        for(int i=0; i <numberBoids; i++)
        {
            GameObject boid_i = Instantiate(boid);
            boid_i.transform.position = Random.insideUnitCircle * spawnAreaBoids;
        }

        //boidsNumberText = sliderBoidsNumber.GetComponentInChildren<TextMeshPro>();

        sliderBoidsNumber.value = numberBoids;
        boidsNumberText.text = numberBoids.ToString();
    }

    public void UpdateBoidsNumber()
    {
        numberBoids = Mathf.RoundToInt(sliderBoidsNumber.value);
        boidsNumberText.text = numberBoids.ToString();
    }
}
