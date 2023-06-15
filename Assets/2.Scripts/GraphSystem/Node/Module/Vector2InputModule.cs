using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Vector2ParamEvent : UnityEvent<Vector2?,int> { }
public class Vector2InputModule : InputModule<Vector2?>
{
    [SerializeField]
    public Vector2ParamEvent destination;

    [SerializeField]
    private int input_index;
    public override void Input(Vector2? input)
    {
        base.Input(input);
        if (input.HasValue)
        {
            destination.Invoke(input, input_index);
        }
    }
}
