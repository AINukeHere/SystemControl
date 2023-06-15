using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantFloat : ConstantNode
{
    [SerializeField]
    private float value;
    [SerializeField]
    private FloatOutputModule floatOutputModule;
    public override void CheckOutput()
    {
        floatOutputModule.Input(value);
    }
}
