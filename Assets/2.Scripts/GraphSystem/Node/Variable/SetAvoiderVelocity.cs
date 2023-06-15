using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAvoiderVelocity : SetVariable<Vector2?>
{
    [SerializeField]
    private Vector2OutputModule vector2_output;
    private Rigidbody2D AvoiderRigid2D;

    public override void Awake()
    {
        base.Awake();
        GameObject avoider = GameObject.FindGameObjectWithTag("Avoider");
        if (avoider != null)
            AvoiderRigid2D = avoider.GetComponentInChildren<Rigidbody2D>();
    }
    public override void CheckOutput()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log($"{name} CheckOutput()");
#endif
        if (isActive >= 2 && value != null)
        {
#if UNITY_EDITOR
            if (gameObject.name.EndsWith("(Test)"))
                Debug.Log($"{name} CheckOutput() : " + value.Value);
#endif
            AvoiderRigid2D.velocity = value.Value;
            vector2_output.Input(AvoiderRigid2D.velocity);
            active_output.Active();
            isActive--;
        }
    }
}