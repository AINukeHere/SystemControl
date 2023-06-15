using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPosUI : MonoBehaviour {

    Text textUI;
    Transform cameraTr;

	void Awake ()
    {
        textUI = GetComponent<Text>();
        cameraTr = GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>().transform;
	}
	
	void FixedUpdate ()
    {
        textUI.text = "(" + cameraTr.position.x.ToString("N") + "," + cameraTr.position.y.ToString("N") + ")";
	}
}
