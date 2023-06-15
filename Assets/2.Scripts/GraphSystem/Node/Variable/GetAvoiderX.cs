using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAvoiderX : GetValue
{
    [SerializeField]
    private FloatOutputModule float_output;
    private Transform avoiderTr;

    public override void Awake()
    {
        base.Awake();
        GameObject avoider = GameObject.FindGameObjectWithTag("Avoider");
        if (avoider != null)
            avoiderTr = avoider.GetComponent<Transform>();
    }
    
    public override void CheckOutput()
    {
        float_output.Input(avoiderTr.localPosition.x);
    }
}
