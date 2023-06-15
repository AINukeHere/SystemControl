using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLossyScaleToOne : MonoBehaviour {
    void Update()
    {
        transform.localScale = new Vector3(
            1 / transform.parent.lossyScale.x,
            1 / transform.parent.lossyScale.y,
            1 / transform.parent.lossyScale.z
            );
    }
}
