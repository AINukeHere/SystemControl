using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntParamEvent : UnityEvent<int?,int> { }
public class IntInputModule : InputModule<int?>
{
    [SerializeField]
    public IntParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(int? input)
    {
        base.Input(input);
        if (input != null)
        {
            destination.Invoke(input,input_index);
        }
    }
}
