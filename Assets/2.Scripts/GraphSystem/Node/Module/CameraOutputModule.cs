using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOutputModule : OutputModule<Camera>
{
    public override void ExpandDisplay()
    {
        textMesh.text = input != null ? input.name : "값없음";
    }
}