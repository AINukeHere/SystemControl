using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedArrowImage : MonoBehaviour {
    [SerializeField]
    private Transform beginTr;
    [SerializeField]
    private Transform endTr;

    void Update()
    {
        FixPosition();
    }
    void FixPosition()
    {
        Vector2 v = endTr.position - beginTr.position;
		transform.position = Vector2.Lerp(beginTr.position,endTr.position,0.5f);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(v.y, v.x) / Mathf.Deg2Rad);
        transform.localScale = Vector3.one * v.magnitude*0.5f;
    }
}
