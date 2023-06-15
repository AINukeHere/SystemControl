using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreGameManager : MonoBehaviour
{
    private static ScoreGameManager Instance;
    public static ScoreGameManager instance
    {
        get
        {
            return Instance;
        }
    }

    //수정해야할 UI
    public Text scoreUI;
    public Text IPM;
    public Image toogleEyeImg;

    public bool isExpandDisplay;
    public Sprite eye_closed_img;
    public Sprite eye_opened_img;



    //ipm 측정변수
    public BigInt[] ipm_check = new BigInt[60];
    public int ipm_last_update_idx = 0;

    float elapsed_time;

    void Awake()
    {
        if (instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        isExpandDisplay = false;
        toogleEyeImg.sprite = eye_closed_img;
        StartCoroutine(CalculateIPM());
    }
    
    public void AddScore(int num)
    {
        AddScore(new BigInt(num.ToString()));
    }
    public void AddScore(string num)
    {
        AddScore(new BigInt(num));
    }
    public void AddScore(BigInt num)
    {
        scoreUI.text = (new BigInt(scoreUI.text) + num).ToString();
        ipm_check[ipm_last_update_idx] = ipm_check[ipm_last_update_idx] + num;
    }
    public void SubtractScore(int num)
    {
        SubtractScore(new BigInt(num.ToString()));
    }
    public void SubtractScore(string num)
    {
        SubtractScore(new BigInt(num));
    }
    public void SubtractScore(BigInt num)
    {
        scoreUI.text = (new BigInt(scoreUI.text) - num).ToString();
        ipm_check[ipm_last_update_idx] = ipm_check[ipm_last_update_idx] - num;
    }

    void Update()
    {
        elapsed_time += Time.deltaTime;
        if (elapsed_time >= 1)
        {
            elapsed_time -= 1;
            if (++ipm_last_update_idx >= 60)
                ipm_last_update_idx -= 60;
            ipm_check[ipm_last_update_idx] = new BigInt();
        }

        
    }
    IEnumerator CalculateIPM()
    {
        while(true)
        {
            BigInt ipm = new BigInt();
            foreach (BigInt i in ipm_check)
                ipm += i;
            IPM.text = ipm.ToString();
            IPMManager.Instance.UpdateHighIPM(gameObject, ipm);
            yield return new WaitForSeconds(1.0f);
        }
    }
    public void Btn_BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
