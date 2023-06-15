using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNodeCameraX : SetVariable<float?>
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
        if (isActive >= 2 && value.HasValue)
        {
            Vector3 temp = cameraTr.position;
            temp.x = value.Value;
            cameraTr.position = temp;
            float_output.Input(temp.x);
            active_output.Active();
            isActive--;
        }
    }
}
