using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Int : Operator<int?, int?>
{
    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null)
        {
            output[0] = input[0] + input[1];
            outputModules[0].Input(output[0]);
        }
    }
}
