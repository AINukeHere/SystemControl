using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAvoiderScale : SetVariable<Vector2?>
{
    [SerializeField]
    private Vector2OutputModule vector2_output;

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
            AvoiderTr.localScale = value.Value;;
            vector2_output.Input(AvoiderTr.localScale);
            active_output.Active();
            isActive--;
        }
    }
}
