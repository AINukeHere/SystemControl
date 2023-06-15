using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider_Enemy : MonoBehaviour
{

    private Vector3 start_pos, return_pos;
    [SerializeField]
    private Transform destination;
    public float speed;

    private bool isFrontGoing;
    void Awake()
    {
        isFrontGoing = true;
        start_pos = transform.position;
        return_pos = destination.position;
    }

    void Update()
    {
        Vector3 v;
        
        if (isFrontGoing)
        {
            v = return_pos - transform.position;
            v.Normalize();

            if (Vector3.Distance(transform.position, return_pos) < speed * Time.deltaTime)
                isFrontGoing = false;
            transform.Translate(v * speed * Time.deltaTime);
        }
        else
        {
            v = start_pos - transform.position;
            v.Normalize();

            if (Vector3.Distance(transform.position, start_pos) < speed * Time.deltaTime)
                isFrontGoing = true;
            transform.Translate(v * speed * Time.deltaTime);
        }
    }
}
