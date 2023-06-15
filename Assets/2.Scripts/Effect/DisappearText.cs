using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearText : MonoBehaviour {

    public string text;
    private TextMesh myTextMesh;
    void Awake()
    {
        Destroy(gameObject, 2);
        myTextMesh = GetComponent<TextMesh>();
    }
	
	void Update ()
    {
        transform.Translate(Vector2.up * Time.deltaTime);
        myTextMesh.text = text;
    }
}
