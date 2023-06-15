using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighIPM : MonoBehaviour {

    Text IPM_text;
    void Awake()
    {
        IPM_text = GetComponent<Text>();
    }
    void OnEnable()
    {
        StartCoroutine(UpdateIPM());
    }
    IEnumerator UpdateIPM()
    {
        while(true)
        {
            IPM_text.text = "Your High IPM : " + IPMManager.Instance.GetHighIPMClone(gameObject).ToString();
            yield return new WaitForSeconds(1.0f);
        }
    }
}
