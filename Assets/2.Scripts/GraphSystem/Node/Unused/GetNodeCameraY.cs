using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNodeCameraY :GetValue
{
    [SerializeField]
    private FloatOutputModule float_output;
    private Transform cameraTr;

    public override void Awake()
    {
        base.Awake();
        cameraTr = GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>().transform;
    }
    public override void CheckOutput()
    {
        float_output.Input(cameraTr.position.y);
    }
}
