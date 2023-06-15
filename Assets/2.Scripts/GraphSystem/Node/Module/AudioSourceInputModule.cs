using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AudioSourceParamEvent : UnityEvent<AudioSource, int> { }
public class AudioSourceInputModule : InputModule<AudioSource>
{
    [SerializeField]
    public AudioSourceParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(AudioSource input)
    {
        base.Input(input);
        if (input != null)
        {
            destination.Invoke(input,input_index);
        }
    }
}
