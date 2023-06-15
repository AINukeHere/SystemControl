using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoiderTempScript : MonoBehaviour {

    Rigidbody2D rigid;
    public bool bCall = false;
    public int w, h;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (bCall)
        {
            bCall = false;
            Screen.SetResolution(w, h, false);
        }
        Debug.Log(rigid.velocity + "at Update");
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Debug.Log(rigid.velocity + "at LateUpdate");
    }
    void FixedUpdate()
    {
        Debug.Log(rigid.velocity + "at FixedUpdate");
    }

}
