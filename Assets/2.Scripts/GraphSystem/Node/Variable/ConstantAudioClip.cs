using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantAudioClip : ConstantNode
{
    [SerializeField]
    private AudioClip value = null;
    [SerializeField]
    private AudioClipOutputModule audioClipOutputModule;
    public override void CheckOutput()
    {
        audioClipOutputModule.Input(value);
    }
}