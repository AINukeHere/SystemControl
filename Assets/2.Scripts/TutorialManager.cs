using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Reflection;

public class TutorialManager : MonoBehaviour
{

    private static TutorialManager _instance;
    public static TutorialManager instance
    {
        get
        {
            return _instance;
        }
    }

    [System.Serializable]
    public class DialogInfo
    {
        //대화상자 내용
        [TextArea]
        public string text;
        //Controller 사용 유무
        public bool bActiveController;
        //다음 대화창 조건
        public ConditionType conditionType;
        //마우스 이동이라던지 인터페이스 UI
        public GameObject tutorialEffect;
    }

    //Tutorial Dialog 정보
    [SerializeField]
    private DialogInfo[] dialogs;
    //Tutorial Dialog 인덱스
    [SerializeField]
    private int dialogIdx = -1;


    [SerializeField]
    private NodeArray[] StageOpenNodes;

    [SerializeField]
    private NodeArray[] StageCloseNodes;


    //Assigning Objects;
    [SerializeField]
    private Image DialogBox;
    private Text DialogBoxText;
    [SerializeField]
    private AudioSource dialogBoxSound;
    private GameObject controller;
    private Camera NodeCamera;
    [SerializeField]
    private GameObject movement_folder;
	private Controller ctrler;
    [SerializeField]
    private PanelMove panelMove;
    [SerializeField]
    private DisplayMsg displayMsg;

    //대화상자 반짝이는 효과 타임라인값 n -> 0
    float dialogEffectTimeline = 0;
    //다음 대화상자 넘어가기까지 기다리는 시간 (목표달성시 바로넘어가지않고 몇초간 대기했다가 넘어감)
    [SerializeField]
    private float DialogBoxDelayTime;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);


        //Assigning
        controller = GameObject.FindGameObjectWithTag("Controller");
        NodeCamera = GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>();
        ctrler = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();

        //Init
        DialogBoxText = DialogBox.GetComponentInChildren<Text>();
        DialogBoxText.supportRichText = true;
        dialogIdx = 0;
        current_stage_num = 0;
        //1스테이지 맵 Active
        StartStage(1);
        //튜토리얼 시작
        StartCoroutine(ShowTutorial(dialogIdx));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ConditionSatisfied(ConditionType.SPACE_KEY_PRESS);

        if (Input.GetKeyDown(KeyCode.Tab))
            ConditionSatisfied(ConditionType.TAB_KEY_PRESS);

        if (Input.GetMouseButtonDown(0))
            ConditionSatisfied(ConditionType.MOUSE_LCLICK);

        if (Input.mouseScrollDelta.magnitude > 0.0001)
            ConditionSatisfied(ConditionType.MOUSE_WHEEL);

        if (NodeCamera.transform.position.y < 4985 && NodeCamera.transform.position.x < 4980)
            ConditionSatisfied(ConditionType.CAMERA_MOVE);

        if(movement_folder == null)
            ConditionSatisfied(ConditionType.FOLDER_UNFOLD);

        if(ctrler.bActiveNodeInfo())
            ConditionSatisfied(ConditionType.SEE_NODE_INFO);

		if (NodeManager.instance.isExpandDisplay) {
			ConditionSatisfied (ConditionType.OPEN_EYE);
		}


        DialogEffect();
    }

    void DialogEffect()
    {
        if (dialogEffectTimeline > 0)
            dialogEffectTimeline -= Time.deltaTime * 1.2f;
        if ((int)dialogEffectTimeline % 2 == 0)
            DialogBox.color = Color.white;
        else
            DialogBox.color = Color.yellow;
    }

    IEnumerator ShowTutorial(int dialogIdx)
    {
        yield return new WaitForSeconds(DialogBoxDelayTime);
        try
        {
            DialogBoxText.text = dialogs[dialogIdx].text;
            controller.SetActive(dialogs[dialogIdx].bActiveController);
            dialogBoxSound.Play();
            dialogEffectTimeline = 6;
            if (dialogIdx > 0 && dialogs[dialogIdx - 1].tutorialEffect != null)
                dialogs[dialogIdx - 1].tutorialEffect.SetActive(false);
            if (dialogs[dialogIdx].tutorialEffect != null)
                dialogs[dialogIdx].tutorialEffect.SetActive(true);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("IndexOutofRangeException 발생");
        }
    }
    [Serializable]
    public enum ConditionType
    {
		NONE,				//어떤 곳에서도 이 조건을 만족시키지 않음.
        SPACE_KEY_PRESS,    //스페이스를 눌러 계속
        TAB_KEY_PRESS,      //텝을 눌러 계속
        BAR_UP,             //BottomLine을 반 이상 올려서 계속
        MOUSE_LCLICK,       //마우스 좌클릭을 하여 계속
        MOUSE_WHEEL,        //마우스 휠을 돌려 계속
        FOLDER_UNFOLD,      //폴더를 해체해 계속
        CAMERA_MOVE,        //카메라좌표를 이동시켜 계속
        SEE_NODE_INFO,      //노드설명을 띄워 게속
		OPEN_EYE,			//눈을 떠서 계속
		RESET_NODE,			//리셋노드 아이콘을 눌러 계속
        STAGE_CLEAR,        //스테이지를 클리어해 계속
		CONNECT_NODE1,		//상수노드와 함수노드를 연결(함수노드에서 값을 받는지로 체크)
		CONNECT_NODE2		//이벤트노드와 함수노드를 연결(함수노드에서 값을 받는지로 체크)
    }
    public void ConditionSatisfied(ConditionType type)
    {
        if (dialogIdx+1 < dialogs.Length && dialogs[dialogIdx].conditionType == type)
        {
            dialogIdx++;
            StartCoroutine(ShowTutorial(dialogIdx));
            if (type == ConditionType.CONNECT_NODE2)
            {
                panelMove.isOpened = true;
                panelMove.isAutoMoving = true;
            }
        }
	}
	public void NodeGotValue(string nodeName, Type type)
	{
		if (nodeName.Equals("Avoider Velocity(Set)"))
			ConditionSatisfied (ConditionType.CONNECT_NODE1);
	}
	public void NodeActivated(string nodeName)
	{
		if (nodeName.Equals("Avoider Velocity(Set)"))
			ConditionSatisfied (ConditionType.CONNECT_NODE2);
	}


    //게임관련변수
    private int current_stage_num;
    //스테이지 게임 오브젝트
    [SerializeField]
    private GameObject[] Stages;
    //Avoider로부터 EndZone에 들어갔을 때 호출되는 함수입니다.
    public void StageClear()
    {
        if (StartStage(current_stage_num + 1))
        {
            current_stage_num++;
            AvoiderPositionReset();
            ConditionSatisfied(ConditionType.STAGE_CLEAR);
        }
    }
    public bool StartStage(int stageNum)
    {
        Debug.Log("StartStage(" + stageNum + "), " + dialogIdx);
        if(stageNum == 2 && dialogIdx < 17)
        {
            return false;
        }
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
            catch (NullReferenceException e)
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
                    displayMsg.ShowMsg("새로운 시스템노드가 추가되었습니다.");
                }
            }

            //없어지는 노드 있으면 활성화
            foreach (GameObject node in StageCloseNodes[stageNum - 1].nodes)
            {
                if (node != null)
                {
                    node.SetActive(false);
                    displayMsg.ShowMsg("일부 시스템노드가 제거되었습니다.");
                }
            }

            current_stage_num = stageNum;
        }
        return true;
    }
    public void AvoiderPositionReset()
    {
        GameObject.FindGameObjectWithTag("Avoider").transform.position = GetStartPosition();
    }
    public Vector3 GetStartPosition()
    {
        return GameObject.FindGameObjectWithTag("StartPosition").transform.position;
    }

    public void Btn_BackHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

}