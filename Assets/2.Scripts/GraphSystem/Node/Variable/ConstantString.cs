using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantString : ConstantNode
{
    [SerializeField]
    private string value;
    [SerializeField]
    private StringOutputModule stringOutputModule;
    public override void CheckOutput()
    {
        stringOutputModule.Input(value);
    }
}
