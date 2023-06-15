using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    [SerializeField]
    private RawImage gameRenderer;
    [SerializeField]
    private Camera nodeCamera;
    private void Awake()
    {
        nodeCamera = GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>();
    }
}
