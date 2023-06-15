using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFixing : MonoBehaviour {

    RectTransform myRectTr;
    Vector3 origin_pos;
    void Awake()
    {
        myRectTr = GetComponent<RectTransform>();
        origin_pos = myRectTr.position;
    }
    void Update()
    {
        myRectTr.position = origin_pos;
    }
    void FixedUpdate()
    {
        myRectTr.position = origin_pos;
    }
    void LateUpdate()
    {
        myRectTr.position = origin_pos;
    }
}
