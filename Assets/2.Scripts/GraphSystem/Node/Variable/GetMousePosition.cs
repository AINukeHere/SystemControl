using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePosition : GetValue
{
    [SerializeField]
    private Vector3OutputModule vector3_output;
    
    public override void CheckOutput()
    {
        vector3_output.Input(Input.mousePosition);
    }
}
