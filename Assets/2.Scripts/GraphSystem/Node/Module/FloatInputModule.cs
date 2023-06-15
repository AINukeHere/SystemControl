using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatParamEvent : UnityEvent<float?,int> { }
public class FloatInputModule : InputModule<float?>
{
    [SerializeField]
    public FloatParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(float? input)
    {
        base.Input(input);
        if (input.HasValue)
        {
            destination.Invoke(input, input_index);
        }
    }
}
