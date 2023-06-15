using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RaycastHitParamEvent : UnityEvent<RaycastHit?,int> { }
public class RaycastHitInputModule : InputModule<RaycastHit?>
{
    [SerializeField]
    public RaycastHitParamEvent destination;

    [SerializeField]
    private int input_index;
    public override void Input(RaycastHit? input)
    {
        base.Input(input);
        if (input.HasValue)
        {
            destination.Invoke(input, input_index);
        }
    }
}
