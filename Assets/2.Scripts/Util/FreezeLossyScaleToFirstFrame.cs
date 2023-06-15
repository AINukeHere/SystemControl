using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeLossyScaleToFirstFrame : MonoBehaviour {
    [SerializeField] private bool freezeX, freezeY, freezeZ;
    private Vector3 originLossyScale;
    private void Awake()
    {
        originLossyScale = transform.lossyScale;
    }
    void Update()
    {
        if(transform.parent == null)
        {
            transform.localScale = new Vector3(
                freezeX ? originLossyScale.x : transform.lossyScale.x,
                freezeY ? originLossyScale.y : transform.lossyScale.y,
                freezeZ ? originLossyScale.z : transform.lossyScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                freezeX ? originLossyScale.x / transform.parent.lossyScale.x : transform.localScale.x,
                freezeY ? originLossyScale.y / transform.parent.lossyScale.y : transform.localScale.y,
                freezeZ ? originLossyScale.z / transform.parent.lossyScale.z : transform.localScale.z
            );
        }
    }
}
