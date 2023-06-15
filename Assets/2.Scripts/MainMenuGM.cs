using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuGM : MonoBehaviour
{
    private static MainMenuGM _instance;
    public static MainMenuGM Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject fbm = new GameObject("MainMenuGM");
                return fbm.AddComponent<MainMenuGM>();
            }
            else
                return _instance;
        }
    }

    public string[] kind_ofgame;
    int index_of_game = 0;

    public Text gameText;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(gameObject);
		gameText.text = gameText.text = kind_ofgame[index_of_game];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    public void Btn_Play()
    {
        SceneManager.LoadScene(kind_ofgame[index_of_game]);
    }
    public void Btn_UpdateScoreAtPlayerPref()
    {
        PlayerPrefs.SetString("IPM", IPMManager.Instance.GetHighIPMClone(gameObject).ToString());
    }
    public void Btn_RightArrow()
    {
        index_of_game = (index_of_game + 1) % kind_ofgame.Length;
        gameText.text = kind_ofgame[index_of_game];
    }
    public void Btn_LeftArrow()
    {
        index_of_game--;
        if (index_of_game < 0)
            index_of_game += kind_ofgame.Length;

        gameText.text = kind_ofgame[index_of_game];
    }
	public void Btn_Exit()
	{
		Application.Quit ();
	}
}
