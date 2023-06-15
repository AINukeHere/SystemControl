using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAvoiderY : SetVariable<float?>
{
    [SerializeField]
    private FloatOutputModule float_output;

    private Transform AvoiderTr;

    public override void Awake()
    {
        base.Awake();
        GameObject avoider = GameObject.FindGameObjectWithTag("Avoider");
        if (avoider != null)
            AvoiderTr = avoider.GetComponent<Transform>();
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && value != null)
        {
            Vector3 temp = AvoiderTr.localPosition;
            temp.y = value.Value;
            AvoiderTr.localPosition = temp;
            float_output.Input(temp.y);
            active_output.Active();
            isActive--;
        }
    }
}
