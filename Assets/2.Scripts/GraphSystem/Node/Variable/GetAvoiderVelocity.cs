using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAvoiderVelocity : GetValue
{
    [SerializeField]
    private Vector2OutputModule vector2_output;
    private Rigidbody2D avoiderRigid2D;

    public override void Awake()
    {
        base.Awake();
        GameObject avoider = GameObject.FindGameObjectWithTag("Avoider");
        if (avoider != null)
            avoiderRigid2D = avoider.GetComponent<Rigidbody2D>();
    }
    public override void CheckOutput()
    {
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("GetAvoiderVelocity CheckOutput() : "+avoiderRigid2D.velocity);
        vector2_output.Input(avoiderRigid2D.velocity);
    }
}
