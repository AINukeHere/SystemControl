using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolParamEvent : UnityEvent<bool?,int> { }
public class BoolInputModule : InputModule<bool?>
{
    [SerializeField]
    public BoolParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(bool? input)
    {
        base.Input(input);
        if (input.HasValue)
        {
            destination.Invoke(input, input_index);
        }
    }
}
