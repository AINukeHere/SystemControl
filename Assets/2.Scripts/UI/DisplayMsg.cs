using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMsg : MonoBehaviour {

    Text text;
    Image myImage;
    float elapsed_time = 0;
    bool bShow;

	void Awake ()
    {
        text = GetComponentInChildren<Text>();
        myImage = GetComponent<Image>();
        text.color = new Color(0, 0, 0, 0);
        myImage.color = new Color(160/256f, 244/256f, 212/256f, 0);
    }

    void Update()
    {
        if(bShow)
        {
            elapsed_time += Time.deltaTime;
            if(elapsed_time >=5 )
                bShow = false;
        }
        else
        {
            text.color = Color.Lerp(text.color, new Color(0,0,0, 0), 0.05f);
            myImage.color = Color.Lerp(myImage.color, new Color(160 / 256f, 244 / 256f, 212 / 256f, 0), 0.05f);
        }
    }
    public void ShowMsg(string msg)
    {
        text.text = msg;
        text.color = new Color(0, 0, 0, 1);
        myImage.color = new Color(160 / 256f, 244 / 256f, 212 / 256f, 1);
        bShow = true;
    }
}
