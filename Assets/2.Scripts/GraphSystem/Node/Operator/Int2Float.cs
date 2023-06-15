using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int2Float : Operator<int?, float?>
{
    public override void CheckOutput()
    {
        if (input[0] != null)
        {
            output[0] = float.Parse(input[0].ToString());
            outputModules[0].Input(output[0]);
        }
    }
}
