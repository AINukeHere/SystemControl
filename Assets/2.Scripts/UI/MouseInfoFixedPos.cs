using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInfoFixedPos : MonoBehaviour {
	
	[SerializeField]
	private bool bFixedPosition;
	[SerializeField]
	private Transform fixedTr;

	//assign objects
	private Camera nodeCamera;
	void Awake () 
	{
		nodeCamera = GameObject.FindGameObjectWithTag ("NodeCamera").GetComponent<Camera>();
	}
	void Update()
	{
		if (bFixedPosition) {
			transform.position = nodeCamera.WorldToScreenPoint (fixedTr.position);
		}
	}
}
