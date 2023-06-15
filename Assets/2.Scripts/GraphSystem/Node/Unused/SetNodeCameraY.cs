using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNodeCameraY : SetVariable<float?>
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
            temp.y = value.Value;
            cameraTr.position = temp;
            float_output.Input(temp.y);
            active_output.Active();
            isActive--;
        }
    }
}
