using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMainCameraPos : GetValue
{
    [SerializeField]
    private Vector3OutputModule vector3_output;
    private Transform cameraTr;

    public override void Awake()
    {
        base.Awake();
        cameraTr = Camera.main.transform;
    }
    
    public override void CheckOutput()
    {
        vector3_output.Input(cameraTr.position);
    }
}
