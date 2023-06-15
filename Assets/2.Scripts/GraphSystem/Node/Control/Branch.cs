using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : ActivatableNode
{ 
    public bool? value;
    public ActiveOutputModule active_output1, active_output2;
    public void Update()
    {
        value = null;
    }

    public override void Active()
    {
#if UNITY_EDITOR
        if (name.EndsWith("(Test)"))
            Debug.Log("BrenchActive");
#endif
        base.Active();
    }
    public void Input(bool? input, int unused = 0)
    {
        if (input.HasValue)
        {
#if UNITY_EDITOR
            if (name.EndsWith("(Test)"))
                Debug.Log("BrenchInput : " + input.ToString());
#endif
            value = input;
            CheckOutput();
        }
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && value.HasValue)
        {
#if UNITY_EDITOR
            if (name.EndsWith("(Test)"))
                Debug.Log("CheckOutput : " + value);
#endif
            if (value.Value)
                active_output1.Active();
            else
                active_output2.Active();
            isActive--;
        }
    }
}
