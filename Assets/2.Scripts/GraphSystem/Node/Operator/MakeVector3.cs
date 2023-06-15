using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVector3 : Operator<float?,Vector3?>
{
    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null && input[2] != null)
        {
            output[0] = new Vector3(input[0].Value, input[1].Value, input[2].Value);
            outputModules[0].Input(output[0]);
            input[0] = null;
            input[1] = null;
            input[2] = null;
        }
    }
}
