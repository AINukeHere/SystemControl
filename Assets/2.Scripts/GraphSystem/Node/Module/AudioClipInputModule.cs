using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AudioClipParamEvent : UnityEvent<AudioClip,int> { }
public class AudioClipInputModule : InputModule<AudioClip>
{
    [SerializeField]
    public AudioClipParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(AudioClip input)
    {
        base.Input(input);
        if (input != null)
        {
            destination.Invoke(input,input_index);
        }
    }
    public override void ExpandDisplay()
    {
        textMesh.text = input_for_visualize != null ? input_for_visualize.name : "값없음";
    }
}
