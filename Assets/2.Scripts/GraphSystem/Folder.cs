using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder : NotInputNotNodeActivatable,IHasInfo
{
    public GameObject[] folded_objects;
    public static Vector3 hiding_vector;
    public bool isHold = false;
    public bool ActiveOnAwake = true;
    public bool isClickable = true;
    public override void Awake()
    {
		base.Awake ();
        hiding_vector = new Vector3(99999, 99999, GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>().transform.position.z - 1);
        if (!isHold)
        {
            isHold = true;
            foreach (GameObject obj in folded_objects)
            {
                obj.transform.position += hiding_vector;
                obj.transform.SetParent(transform);
            }
        }
        gameObject.SetActive(ActiveOnAwake);
        EdgeManager.instance.UpdateAll();
    }
    public override void UpdateDisplay()
    {
        if(isClickable)
            base.UpdateDisplay();
    }
    public override void Active()
    {
        if (isClickable)
            base.Active();
    }
    public void Update()
    {
        //담고있는 노드들이 있다면
        if (folded_objects.Length > 0)
        {
            TouchEvent touchInput = folded_objects[0].GetComponent<TouchEvent>();
            if (touchInput != null && isActive >= 2)
                touchInput.Active();
        }
        //버튼처럼 클릭
        if (isClickable && isActive >= 1)
        {
            myRenderer.color = new Color(200 / 256.0f, 200 / 256.0f, 200 / 256.0f);
            isActive--;
        }
        else
            myRenderer.color = new Color(1, 1, 1);
    }
    public void UnFold()
    {
        foreach (GameObject obj in folded_objects)
        {
            obj.transform.position -= hiding_vector;
            obj.transform.SetParent(null);
        }
        Destroy(gameObject);
        EdgeManager.instance.UpdateAll();
    }

    [SerializeField]
    private string infoMsg;
    public string GetInfoString()
    {
        return infoMsg;
    }
}
