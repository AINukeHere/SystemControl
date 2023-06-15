using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHorizontalInput : GetValue
{
    [SerializeField]
    private FloatOutputModule float_output;

    public override void CheckOutput()
    {
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("GetHorizontalInput CheckOutput()");
        float_output.Input(Input.GetAxisRaw("Horizontal"));
    }
}
