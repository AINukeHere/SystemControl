using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //Assigned Data
    [SerializeField]
    private Transform SelectBoxTr;
    [SerializeField]
    private SpriteRenderer SelectBoxRenderer;
    [SerializeField]
    private GameObject movableIconObj;
    [SerializeField]
    private FrameHighlighting frameHighlighting;
    private AudioSource myAudio;
    private PanelMove panelMove;

    //clicked = 클릭을 하고 있는가  //의미가 없는 변수들이긴 함 Input.GetMouseButton(x)으로 대체가능하기때문
    [SerializeField]
    private bool Lclicked;
    [SerializeField]
    private bool Rclicked;

    // 노드를 집으면 노드의 회전값을 정상으로 되돌립니다. (AvoiderEnd씬 전용)
    [SerializeField]
    private bool bNodeRotate = false;
    [SerializeField]
    private float rotateLerp_t = 0.5f;

    private bool isUI;
    private GameObject selectedObject;
    private Vector3 localOffset;
    private Vector2 touchPos;

    //이동시킬 때 그룹인지 단일인지 판단
    private bool isGroup;

    private List<GameObject> selectedObjects;
    private List<Vector3> localOffsets;
    private List<GameObject> moveIcons;

    public Vector3 boxLeftTopPos;

    public bool isGetTool;
    public SpriteRenderer spriteRenderer;

    //좌클릭으로 카메라 이동기능 변수
    public float cameraMoveSpeed = 0.1f;
    public Vector2 startClickedPos;

    void Awake()
    {
        selectedObjects = new List<GameObject>();
        localOffsets = new List<Vector3>();
        moveIcons = new List<GameObject>();

        ResetMultySelectSystems();
        nodeCamera = GameObject.FindGameObjectWithTag("NodeCamera").GetComponent<Camera>();
        myAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SelectBoxTr.localScale = new Vector3(0, 0, 1);
        InfoBoxText = InfoBox.GetComponentInChildren<Text>();
        panelMove = GameObject.Find("BottomLine").GetComponent<PanelMove>();
        InfoBox.SetActive(false);
    }
    void Update()
    {
        // New Edge의 Target을 Controller로 하기로 함
        Vector3 worldPositionMouse = nodeCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPositionMouse.z = 0;
        transform.position = worldPositionMouse;


        AlphaChangeWithTool();
        ProcessLeftMouseClick();
        ProcessRightMouseClick();
        MovableNodeHighlightingAtMouseCursor();
        if (MouseNonMovement() && !Lclicked)
            ShowInfo(GetInfoAtMouseCursor());
        else
            HideInfo();

        if (Input.GetKeyUp(KeyCode.LeftAlt))
            NodeManager.instance.Btn_ToogleEye();
        if (Input.GetKeyDown(KeyCode.Tab))
            panelMove.StartAutoMove();
    }

    void AlphaChangeWithTool()
    {
        if (isGetTool)
        {
            transform.position = (Vector2)nodeCamera.ScreenToWorldPoint(Input.mousePosition);
            Color temp = spriteRenderer.color;
            temp.a = 1;
            spriteRenderer.color = temp;
        }
        else
        {
            Color temp = spriteRenderer.color;
            if (temp.a > 0)
                temp.a -= 0.1f;
            spriteRenderer.color = temp;
        }
    }
    void MovableNodeHighlightingAtMouseCursor()
    {
        Vector3 worldMousePos = nodeCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] overlapColls = Physics2D.OverlapPointAll(worldMousePos);
        Collider2D target=null;
        //ArrowInput,ArrowOutput 우선처리, 없다면 가장 마지막의 것을 채택
        foreach (Collider2D coll in overlapColls)
        {
            NewEdge newEdge = coll.GetComponent<NewEdge>();
            if (newEdge != null)
            {
                target = coll;
                break;
            }
            IMovable movableCode = coll.GetComponent<IMovable>();
            if (movableCode != null)
            {
                target = coll;
                break;
            }
        }

        if (target == null)
            frameHighlighting.gameObject.SetActive(false);
        else
        {
            NewEdge isEdge = target.GetComponent<NewEdge>();
            if (isEdge == null)
            {
                frameHighlighting.gameObject.SetActive(true);
                frameHighlighting.targetTr = target.transform;
            }
            //연결중인 엣지는 하이리팅하지않음.
            else if(!isEdge.endTarget.Equals(transform))
                isEdge.Highlighting();
        }
    }
    void ProcessLeftMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startClickedPos = Input.mousePosition;
            //무엇을 클릭했는지 체크
            //UI기준좌표계에서 충돌체크.
            Collider2D temp0 = Physics2D.OverlapPoint(Input.mousePosition);
            if (temp0 != null)
            {
                selectedObject = temp0.gameObject;
                localOffset = selectedObject.transform.position - Input.mousePosition;
                isUI = true;
            }
            //노드시스템좌표계에서 충돌체크.
            else
            {
                touchPos = nodeCamera.ScreenToWorldPoint(Input.mousePosition);

                //Switch노드의 AddCase, DeleteCase처리
                Collider2D[] temps = Physics2D.OverlapPointAll(touchPos);
                foreach (Collider2D coll in temps)
                {
                    if (coll.gameObject.name == "AddStringCase")
                        coll.gameObject.transform.parent.GetComponent<SwitchOnString>().AddCase();
                    if (coll.gameObject.name == "DeleteStringCase")
                        coll.gameObject.transform.parent.GetComponent<SwitchOnString>().DeleteCase();
                }

                Collider2D temp = null;
                Collider2D[] overlapColls = Physics2D.OverlapPointAll(touchPos);
                //Folder가 있으면 선택, 없으면 가장 마지막의 것을 채택
                foreach (Collider2D coll in overlapColls)
                {
                    Folder folderComp = coll.GetComponent<Folder>();
                    if (folderComp != null)
                    {
                        temp = coll;
                        break;
                    }
                    temp = coll;
                }
                if (temp != null && !temp.CompareTag("LockField"))
                {
                    selectedObject = temp.gameObject;

                    //선택한 오브젝트가 우클릭드래그로 선택한 그룹에 속해있는지 확인
                    isGroup = false;
                    foreach (GameObject go in selectedObjects)
                    {
                        if (go == selectedObject)
                            isGroup = true;
                    }
                    //속해있다면 그 그룹의 localOffset을 구함
                    if (isGroup)
                    {
                        localOffsets.Clear();
                        for (int i = 0; i < selectedObjects.Count; ++i)
                            localOffsets.Add((Vector2)selectedObjects[i].transform.position - touchPos);
                    }
                    else
                        localOffset = (Vector2)selectedObject.transform.position - touchPos;


                }
                isUI = false;
            }
            Lclicked = true;

            //클릭시 바로 일어나야 하는 부분 처리.
            if (selectedObject != null)
            {
                Folder folder = selectedObject.GetComponent<Folder>();
                TouchEvent touchEvent = selectedObject.GetComponent<TouchEvent>();
                if (isGetTool)
                {
                    if (folder != null)
                    {
                        folder.UnFold();
                        myAudio.Play();
                        //myAudio.PlayDelayed(0);
                        isGetTool = false;
                    }
                }
                else
                {
                    if (touchEvent != null)
                        touchEvent.Active();
                    if (folder != null)
                        folder.Active();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Lclicked = false;
            if (isGroup)
            {
                foreach (GameObject _selectedObject in selectedObjects)
                {
                    IMovable movable = _selectedObject.GetComponent<IMovable>();
                    if (movable != null)
                    {
                        if (!isUI)
                            movable.MoveEnd((nodeCamera.ScreenToWorldPoint(Input.mousePosition) + localOffset),true);
                        else
                            movable.MoveEnd(Input.mousePosition + localOffset,true);
                    }
                }
            }
            else if (selectedObject != null)
            {
                IMovable movable = selectedObject.GetComponent<IMovable>();
                if (movable != null)
                {
                    if (!isUI)
                        movable.MoveEnd((nodeCamera.ScreenToWorldPoint(Input.mousePosition) + localOffset));
                    else
                        movable.MoveEnd(Input.mousePosition + localOffset);
                }
            }
            selectedObject = null;
        }
        //클릭되고있는 동안
        else if (Lclicked)
        {
            //선택된 오브젝트가 있다면
            if (selectedObject != null)
            {
                //여러개 선택되었는가
                if (isGroup)
                {
                    for (int i = 0; i < selectedObjects.Count; ++i)
                    {
                        IMovable movable = selectedObjects[i].GetComponent<IMovable>();
                        if (movable != null)
                        {
                            if (!isUI)
                            {
                                movable.Move((nodeCamera.ScreenToWorldPoint(Input.mousePosition) + localOffsets[i]), true);
                                if (bNodeRotate)
                                {
                                    selectedObjects[i].transform.rotation =
                                        Quaternion.Slerp(selectedObjects[i].transform.rotation, Quaternion.identity, rotateLerp_t);
                                    selectedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                                }
                                }
                            else
                                movable.Move(Input.mousePosition + localOffsets[i], true);
                        }
                    }
                }
                else
                {
                    IMovable movable = selectedObject.GetComponent<IMovable>();
                    if (movable != null)
                    {
                        if (!isUI)
                        {
                            movable.Move((nodeCamera.ScreenToWorldPoint(Input.mousePosition) + localOffset));
                            if (bNodeRotate)
                            {
                                selectedObject.transform.rotation =
                                    Quaternion.Slerp(selectedObject.transform.rotation, Quaternion.identity, rotateLerp_t);
                                selectedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            }
                        }
                        else
                            movable.Move(Input.mousePosition + localOffset);
                    }
                }
            }
            else
            {
                ResetMultySelectSystems();
                //화면이동
                if (Input.GetMouseButton(0))
                {
                    Vector2 delta = (Vector2)Input.mousePosition - startClickedPos;
                    nodeCamera.transform.position -= new Vector3(delta.x, delta.y) * cameraMoveSpeed * Time.deltaTime * nodeCamera.orthographicSize;
                }
            }
        }
        if(Application.isFocused)
            startClickedPos = Input.mousePosition;
    }
    void ProcessRightMouseClick()
    {
        //우클릭 시작
        if (Input.GetMouseButtonDown(1))
        {
            isGetTool = false;
            ResetMultySelectSystems();
            boxLeftTopPos = nodeCamera.ScreenToWorldPoint(Input.mousePosition);
            Rclicked = true;
            //엣지 끊기
            touchPos = nodeCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] temps = Physics2D.OverlapPointAll(touchPos);
            foreach (Collider2D temp in temps)
            {
                NewEdge edgeComponent = temp.GetComponent<NewEdge>();
                //엣지이면서 타겟이 마우스가 아닐때(아직 마우스로 연결중이면 무시)
                if (edgeComponent != null && !edgeComponent.endTarget.Equals(transform))
                {
                    EdgeManager.instance.removeEdge(edgeComponent, edgeComponent.endTarget);
                }
            }

        }
        if (Input.GetMouseButton(1))
        {
            //크기조절
            float xSize, ySize;
            Vector3 v3 = nodeCamera.ScreenToWorldPoint(Input.mousePosition);
            xSize = v3.x - boxLeftTopPos.x;
            ySize = v3.y - boxLeftTopPos.y;
            SelectBoxTr.localScale = new Vector3(xSize * 0.5f, ySize * 0.5f, 1);
            Vector3 boxOffset = boxLeftTopPos - transform.position;
            SelectBoxTr.localPosition = new Vector3(boxOffset.x, boxOffset.y,0);

            //LockField가 있으면 빨간색으로 표시해줌
            Collider2D[] colls = Physics2D.OverlapAreaAll(SelectBoxTr.position, SelectBoxTr.position + SelectBoxTr.lossyScale * 2);
            int i;
            for (i = 0; i < colls.Length; ++i)
            {
                if (colls[i].CompareTag("LockField"))
                {
                    SelectBoxRenderer.color = new Color(1, 0, 0, 0.5f);
                    break;
                }
            }
            if (i == colls.Length)
                SelectBoxRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Collider2D[] colls = Physics2D.OverlapAreaAll(SelectBoxTr.position, SelectBoxTr.position + SelectBoxTr.lossyScale * 2);

            //LockField가 껴있는지 체크
            int i;
            for (i = 0; i < colls.Length; ++i)
            {
                if (colls[i].CompareTag("LockField"))
                {
                    SelectBoxRenderer.color = new Color(1, 0, 0, 0.5f);
                    break;
                }
            }
            if (i == colls.Length)
            {
                //콜라이더들 중 Movable인터페이스를 구현한 게임오브젝트 숫자를 구함
                int movableCount = 0;
                foreach (Collider2D coll in colls)
                {
                    IMovable temp = coll.GetComponent<IMovable>();
                    if (temp != null)
                        movableCount++;
                }

                selectedObjects.Clear();// = new GameObject[movableCount];
                moveIcons.Clear();// = new GameObject[movableCount];
                foreach (Collider2D coll in colls)
                {
                    IMovable temp = coll.GetComponent<IMovable>();
                    if (temp != null)
                    {
                        selectedObjects.Add(coll.gameObject);
                        GameObject obj = Instantiate(movableIconObj, coll.transform.position - new Vector3(0, 0, 1), Quaternion.identity);
                        obj.transform.SetParent(coll.transform);
                        moveIcons.Add(obj);
                    }
                }

                SelectBoxRenderer.color = new Color(1, 0, 0, 0.5f);
            }
            SelectBoxTr.localScale = new Vector3(0, 0, 1);
            Rclicked = false;
        }
    }
    void ResetMultySelectSystems()
    {
        selectedObjects.Clear();
        localOffsets.Clear();
        foreach (GameObject icon in moveIcons)
            Destroy(icon);
        moveIcons.Clear();
    }

    Camera nodeCamera;

    /*********노드 정보 표시하기위한 코드 시작*******/
    string GetInfoAtMouseCursor()
    {
        Collider2D[] colls;

        //덮고있는 UI가 있는지 확인
        colls = Physics2D.OverlapPointAll(Input.mousePosition);
        bool isUIExist = false;
        foreach (Collider2D coll in colls)
        {
            if (coll.gameObject.layer == 5)
            {
                isUIExist = true;
                break;
            }
        }

        //UI가 없을때 Node혹은 Folder에서 infoString반환
        if (!isUIExist)
        {
            colls = Physics2D.OverlapPointAll(nodeCamera.ScreenToWorldPoint(Input.mousePosition));
            foreach (Collider2D coll in colls)
            {
                Folder folder = coll.GetComponent<Folder>();
                if (folder != null)
                    return folder.GetInfoString();

                Node node = coll.GetComponent<Node>();
                if (node != null)
                    return node.GetInfoString();
            }
        }
        return null;
    }
    //마우스 안움직인 시간
    float mouseNonMovementTime = 0;
    //마우스 안움직였다고 판단할 시간
    [SerializeField] private float mouseNonMovementSenseTime = 1;
    //전 프레임의 마우스위치
    Vector3 oldMousePosition = Vector3.zero;
    bool MouseNonMovement()
    {
        //마우스 움직임 체크 & 시간계산
        if ((Input.mousePosition - oldMousePosition).magnitude < 3)
            mouseNonMovementTime += Time.deltaTime;
        else
            mouseNonMovementTime = 0;
        oldMousePosition = Input.mousePosition;

        //판단
        if (mouseNonMovementTime > mouseNonMovementSenseTime)
            return true;
        return false;
    }
    [SerializeField]
    private GameObject InfoBox;
    private Text InfoBoxText;
    string AutoNewLineInfoStr(string info)
    {
        int maxHorizontalText = 20;
        for (int i = maxHorizontalText; i < info.Length; i += (maxHorizontalText + 1))
        {
            info = info.Insert(i, "\n");
        }
        return info;
    }
    void ShowInfo(string infoMsg)
    {
        InfoBox.transform.position = Input.mousePosition;
        if (!InfoBox.activeSelf && infoMsg != null)
        {
            InfoBox.SetActive(true);
            InfoBoxText.text = AutoNewLineInfoStr(infoMsg);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)InfoBoxText.transform);
        }
    }
    //Tutorial Manager에서 사용
    public bool bActiveNodeInfo()
    {
        return InfoBox.activeInHierarchy;
    }
    void HideInfo()
    {
        if (InfoBox.activeSelf)
            InfoBox.SetActive(false);
    }
    /*********노드 정보 표시하기위한 코드 끝*********/


    
    public void Btn_PutTool()
    {
        isGetTool = !isGetTool;
    }
}