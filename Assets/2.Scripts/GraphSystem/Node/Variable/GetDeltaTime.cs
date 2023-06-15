using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDeltaTime : GetValue
{
    [SerializeField]
    private FloatOutputModule float_output;

    public override void CheckOutput()
    {
        float_output.Input(Time.deltaTime);
    }
}
