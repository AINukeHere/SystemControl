using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoiderCollision : Event, IExpandableDisplay
{
    private Transform AvoiderTr;
    public StringOutputModule string_output;

    public override void Awake()
    {
        base.Awake();
        GameObject avoider = GameObject.FindGameObjectWithTag("Avoider");
        if (avoider != null)
            AvoiderTr = avoider.GetComponent<Transform>();
        else
            Debug.LogWarning("avoider not found");
    }
    string collTags;
    public override void Update()
    {
        base.Update();
        collTags = "";
        Transform real_avoider_model = AvoiderTr.GetChild(0);

        Collider2D[] colls;// = new Collider2D[10];
        //int colliderCount = AvoiderTr.GetComponentInChildren<Collider2D>().OverlapCollider(new ContactFilter2D(), colls);
        colls = Physics2D.OverlapAreaAll(real_avoider_model.position - real_avoider_model.lossyScale, real_avoider_model.position + real_avoider_model.lossyScale);
        bool bExistCollider = false;
        for (int i = 0; i< colls.Length; ++i)
        {
            Collider2D coll = colls[i];
            if (!(coll.transform.IsChildOf(AvoiderTr) || coll.transform == AvoiderTr))
            {
                bExistCollider = true;
                string_output.Input(coll.tag);
                active_output.Active();
                Active();
                collTags += (coll.tag + "\n");
            }
        }
        if (!bExistCollider)
            collTags += "값없음";
    }
    public bool isExpanded { get; set; }


    public override void UpdateDisplay()
    {
        base.UpdateDisplay();
        if (isExpanded)
            nodeTitle.text = collTags;
        else
            nodeTitle.text = nodeName;
    }
}
