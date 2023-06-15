using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CameraMove_ForDebug : MonoBehaviour {

    public bool bChaseAvoider;

    private Transform avoiderTr;
    void Awake()
    {
        avoiderTr = GameObject.FindGameObjectWithTag("Avoider").transform;
    }
    void LateUpdate()
    {
        if (bChaseAvoider)
        {
            if (avoiderTr != null)
            {
                Vector3 temp = avoiderTr.position;;
                temp.z = -10;
                transform.position = temp;
            }
            else
                avoiderTr = GameObject.FindGameObjectWithTag("Avoider").transform;
        }
    }
}
