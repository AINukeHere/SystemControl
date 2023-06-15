using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Render_Order_Setting : MonoBehaviour {

    [SerializeField]
    private int orderIndex;

	void Update()
    {
        //transform.SetSiblingIndex(orderIndex);
    }
    public void Sorting()
    {
        GetComponent<RectTransform>().SetSiblingIndex(orderIndex);
        Debug.Log(name + "'s OrderIndex : " + transform.GetSiblingIndex());
    }
}
