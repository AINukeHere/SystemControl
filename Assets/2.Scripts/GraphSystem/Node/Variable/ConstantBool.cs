using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantBool : ConstantNode
{
    [SerializeField]
    private bool value = false;
    [SerializeField]
    private BoolOutputModule boolOutputModule;
    public override void CheckOutput()
    {
        boolOutputModule.Input(value);
    }
}
