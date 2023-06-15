using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAvoiderGravityScale : SetVariable<float?>
{
    [SerializeField]
    private FloatOutputModule float_output;

    private Rigidbody2D AvoiderRigid;

    public override void Awake()
    {
        base.Awake();
        GameObject avoider = GameObject.FindGameObjectWithTag("Avoider");
        if (avoider != null)
            AvoiderRigid = avoider.GetComponent<Rigidbody2D>();
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && value != null)
        {
            AvoiderRigid.gravityScale = value.Value;;
            float_output.Input(AvoiderRigid.gravityScale);
            active_output.Active();
            isActive--;
        }
    }
}
