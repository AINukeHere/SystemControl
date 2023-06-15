using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absolute_Float : Operator<float?, float?>
{
    public override void CheckOutput()
    {
        if (input[0] != null)
        {
            output[0] = Mathf.Abs(input[0].Value);
            outputModules[0].Input(output[0]);
        }
    }
}
