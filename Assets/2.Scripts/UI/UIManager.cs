using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Font font;
    public GameObject Canvas;
    private static UIManager Instance;
    public static UIManager instance
    {
        get
        {
            return Instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    public Text GenTextUI(string text)
    {
        GameObject go = new GameObject("Text");
        Text textComp = go.AddComponent<Text>();
        textComp.text = text;
        textComp.font = font;
        TextAnchor anchor = textComp.alignment;

        go.transform.SetParent(Canvas.transform);
        return textComp;
    }
}

