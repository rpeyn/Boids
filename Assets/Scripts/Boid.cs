using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Rigidbody2D m_Rigidbody;
    public float m_Speed = 2.0f;
    //public GameObject viewCircle;
    Vector2 dir;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody.velocity = Random.insideUnitCircle * m_Speed;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log(other.gameObject.name);
    //    Debug.Log(m_Rigidbody.velocity);
    //    //m_Rigidbody.AddForce(Vector2.right * 2f, ForceMode2D.Impulse);
    //    //m_Rigidbody.velocity = transform.right * m_Speed;
    //    dir = m_Rigidbody.transform.position - other.transform.position;
    //    dir = dir.normalized;
    //    m_Rigidbody.velocity += dir * m_Speed;
    //    Debug.Log(m_Rigidbody.velocity + " 2");
    //}
}
