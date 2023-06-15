using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RayParamEvent : UnityEvent<Ray?,int> { }
public class RayInputModule : InputModule<Ray?>
{
    [SerializeField]
    public RayParamEvent destination;

    [SerializeField]
    private int input_index;
    public override void Input(Ray? input)
    {
        base.Input(input);
        if (input.HasValue)
        {
            destination.Invoke(input, input_index);
        }
    }
}
