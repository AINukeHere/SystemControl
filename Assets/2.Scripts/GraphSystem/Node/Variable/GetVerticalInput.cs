using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVerticalInput : GetValue
{
    [SerializeField]
    private FloatOutputModule float_output;
    public override void CheckOutput()
    {
        float_output.Input(Input.GetAxisRaw("Vertical"));
    }
}
