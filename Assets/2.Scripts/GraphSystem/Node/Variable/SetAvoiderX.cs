using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAvoiderX : SetVariable<float?>
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
            temp.x = value.Value;
            AvoiderTr.localPosition = temp;
            float_output.Input(temp.x);
            active_output.Active();
            isActive--;
        }
    }
}
