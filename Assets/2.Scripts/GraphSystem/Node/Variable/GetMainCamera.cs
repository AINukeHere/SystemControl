using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMainCamera : GetValue
{
    [SerializeField]
    private CameraOutputModule camera_output;
    private Camera mainCamera;

    public override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }
    
    public override void CheckOutput()
    {
        camera_output.Input(mainCamera);
    }
}
