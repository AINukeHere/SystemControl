using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPMManager : MonoBehaviour {

    private static IPMManager _instance;
    public static IPMManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject fbm = new GameObject("IPMManager");
                fbm.AddComponent<IPMManager>();
                
            }
            return _instance;
        }
    }
    private static BigInt high_ipm = new BigInt(0);
    /// <summary>
    /// 더 높은 점수가 들어오면 갱신함
    /// </summary>
    /// <param name="whoareyou"></param>
    /// <param name="ipm"></param>
    public void UpdateHighIPM(GameObject whoareyou, BigInt ipm)
    {
        high_ipm =  high_ipm < ipm ? ipm : high_ipm;
        PlayerPrefs.SetString("IPM", high_ipm.ToString());
    }
    public BigInt GetHighIPMClone(GameObject whoareyou)
    {
        return high_ipm.Clone();
    }

    void Awake ()
    {
        if(_instance == null)
        {
            _instance = this;
            string saved_ipm = PlayerPrefs.GetString("IPM");
            if (!string.IsNullOrEmpty(saved_ipm))
                IPMManager.Instance.UpdateHighIPM(gameObject, new BigInt(saved_ipm));
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
	

	void Update ()
    {
		
	}
}
