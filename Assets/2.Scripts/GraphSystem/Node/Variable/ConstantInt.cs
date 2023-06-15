using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantInt : ConstantNode
{
    [SerializeField]
    private int value;
    [SerializeField]
    private IntOutputModule intOutputModule;
    public override void CheckOutput()
    {
        intOutputModule.Input(value);
    }
}
