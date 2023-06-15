using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class NodeArray
{
    [SerializeField]
    public GameObject[] nodes;
}
public class AvoidGameManager : MonoBehaviour
{
    private static AvoidGameManager Instance;
    public static AvoidGameManager instance
    {
        get
        {
            return Instance;
        }
    }

    //수정해야할 UI
    public DisplayMsg DisplayMsgUIScript;


    //스테이지 게임 오브젝트
    [SerializeField]
    private GameObject[] Stages;
    [SerializeField]
    private NodeArray[] StageOpenNodes;

    [SerializeField]
    private NodeArray[] StageCloseNodes;

    [SerializeField]
    private Text StageNumUI;


    //게임관련변수
    private int current_stage_num;


    private GameObject avoiderObj;
    void Awake()
    {
        if (instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        current_stage_num = 0;
        avoiderObj = GameObject.FindGameObjectWithTag("Avoider");
    }
    public bool debugMode = true;
    void Update()
    {
        //StageNumUI text Update
        if(StageNumUI != null)
            StageNumUI.text = "Stage " + current_stage_num.ToString();
        //
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartStage(1);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartStage(2);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartStage(3);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartStage(4);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                StartStage(5);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                StartStage(6);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                StartStage(7);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                StartStage(8);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                StartStage(9);
                AvoiderPositionReset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                StartStage(10);
                AvoiderPositionReset();
            }
        }

    }

    public void AvoiderPositionReset()
    {
        avoiderObj.transform.position = GetStartPosition();
    }
    public void AvoiderScaleReset()
    {
        avoiderObj.transform.localScale = Vector3.one;
    }
    public Vector3 GetStartPosition()
    {
        return GameObject.FindGameObjectWithTag("StartPosition").transform.position;
    }
    public void StartStage(int stageNum)
    {
        //스테이지 시작이 가능한 입력이 들어왔는가.
        if (stageNum > 0 && stageNum <= Stages.Length)
        {
            //이전 스테이지 비활성화
            if (current_stage_num != 0)
                Stages[current_stage_num - 1].SetActive(false);
            //새로운 스테이지 활성화
            try
            {
                Stages[stageNum - 1].SetActive(true);
            }
            catch(NullReferenceException e)
            {
                Debug.LogWarning("Stage" + stageNum + "is null");
                Debug.LogError(e.Message);
            }

            //새로운 노드 있으면 활성화
            foreach (GameObject node in StageOpenNodes[stageNum - 1].nodes)
            {
                if (node != null)
                {
                    node.SetActive(true);
                    DisplayMsgUIScript.ShowMsg("새로운 시스템노드가 추가되었습니다.");
                }
            }

            //없어지는 노드 있으면 활성화
            foreach(GameObject node in StageCloseNodes[stageNum - 1].nodes)
            {
                if (node != null)
                {
                    node.SetActive(false);
                    DisplayMsgUIScript.ShowMsg("일부 시스템노드가 제거되었습니다.");
                }
            }
            current_stage_num = stageNum;
            DotConnectManager.instance.ResetWalls();
            DotConnectManager.instance.InitWallWithStageNum(current_stage_num);
        }
    }
    //현재 진행중인 nStage를 가져옵니다.
    public int GetCurrentStageNum(GameObject whoareyou)
    {
        return current_stage_num;
    }
    public GameObject GetCurrenStageObjectParent(GameObject whoareyou)
    {
        return Stages[current_stage_num - 1];
    }

    public void Btn_BackToMenu()
    {
        StartCoroutine(LoadAsyncMainMenuScene());
    }
    IEnumerator LoadAsyncMainMenuScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        //NodeManager.instance.ShutDownAllNode();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        //asyncLoad.allowSceneActivation = false;
        //Debug.Log("로드시작");
        //yield return new WaitForSeconds(3.0f);
        //Debug.Log("대기끝");
        //asyncLoad.allowSceneActivation = true;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //Debug.Log("씬 아직 준비안됨");
            yield return null;
        }
    }
}
