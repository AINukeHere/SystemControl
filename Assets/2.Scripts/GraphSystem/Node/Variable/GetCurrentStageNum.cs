using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCurrentStageNum : GetValue
{
    [SerializeField]
    private IntOutputModule int_output;

    public override void CheckOutput()
    {
        int_output.Input(AvoidGameManager.instance?.GetCurrentStageNum(gameObject));
    }
}
