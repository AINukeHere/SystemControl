using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiply_Float : Operator<float?,float?> {
    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null)
        {
            output[0] = input[0].Value * input[1].Value;
            outputModules[0].Input(output[0]);
            input[0] = null;
            input[1] = null;
        }
    }
}
