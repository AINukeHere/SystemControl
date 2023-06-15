using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CameraParamEvent : UnityEvent<Camera,int> { }
public class CameraInputModule : InputModule<Camera>
{
    [SerializeField]
    public CameraParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(Camera input)
    {
        base.Input(input);
        if (input != null)
        {
            destination.Invoke(input, input_index);
        }
    }
}
