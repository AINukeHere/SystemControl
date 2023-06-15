using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVector2 : Operator<float?,Vector2?>
{
    public override void CheckOutput()
    {
        if (input[0] != null && input[1] != null)
        {
#if UNITY_EDITOR
            if (gameObject.name.EndsWith("(Test)"))
                Debug.Log("MakeVector2 CheckOutput() : made vector");
#endif
            output[0] = new Vector2(input[0].Value, input[1].Value);
            outputModules[0].Input(output[0]);
            input[0] = null;
            input[1] = null;
        }
    }
}
