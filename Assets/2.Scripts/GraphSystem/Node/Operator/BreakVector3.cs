using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakVector3 : Operator<Vector3?,float?>
{
    public override void CheckOutput()
    {
        if (input[0] != null)
        {
            output[0] = input[0].Value.x;
            output[1] = input[0].Value.y;
            output[2] = input[0].Value.z;
            outputModules[0].Input(output[0]);
            outputModules[1].Input(output[1]);
            outputModules[2].Input(output[2]);
        }
    }
}
