using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider_Switch : MonoBehaviour {

    [SerializeField]
    private int index;
    [SerializeField]
    private SpriteRenderer[] connect_line_renderers;
    [SerializeField]
    private Avoider_SwitchWall switch_wall;
    
    void Update()
    {
        bool bflag = false;
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position,transform.lossyScale.x);
        foreach(Collider2D coll in colls)
        {
            if(coll.CompareTag("Avoider"))
            {
                bflag = true;
                break;
            }
        }
        if (bflag || switch_wall.state)
        {
            foreach (SpriteRenderer render in connect_line_renderers)
                render.color = Color.green;
            switch_wall.ReportSwitchState(index, true);
        }
        else
        {
            foreach (SpriteRenderer render in connect_line_renderers)
                render.color = Color.red;
            switch_wall.ReportSwitchState(index, false);
        }
    }
}
