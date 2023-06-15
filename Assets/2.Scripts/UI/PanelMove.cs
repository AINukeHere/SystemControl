using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMove : MonoBehaviour, IMovable
{

    public bool isMoving
    {
        get; set;
    }
    public float min_y, max_y, middle_y;
    public float anchoredX, anchoredY,aspect;
    public Vector2 myPos;

    public Canvas canvas;
    private RectTransform myRectTr;


    public bool isOpened;
    public bool isAutoMoving;
    void Awake()
    {
        isAutoMoving = false;
        myRectTr = GetComponent<RectTransform>();

        //고정된 x값 1920
        anchoredX = canvas.GetComponent<CanvasScaler>().referenceResolution.x;
        //종횡비
        aspect = canvas.pixelRect.yMax / canvas.pixelRect.xMax;
        //가로는 1920 고정 세로는 종횡비로 계산
        anchoredY = anchoredX * aspect;
        max_y = anchoredY - myRectTr.rect.height / 2;
        min_y = 0 + myRectTr.rect.height / 2;
        middle_y = min_y + (max_y - min_y)*0.5f;
        GetComponent<BoxCollider2D>().size = new Vector2(anchoredX, 20);
    }
    void Update()
    {
        myPos = myRectTr.anchoredPosition;
        if (!(isMoving || isAutoMoving))
        {
            Vector2 temp = myRectTr.anchoredPosition;
            temp.y = Mathf.Lerp(temp.y, Mathf.Clamp(myRectTr.anchoredPosition.y, min_y, max_y), 0.5f);
            myRectTr.anchoredPosition = temp;
        }

        //이제 컨트롤러에서 처리
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    StartAutoMove();
        //}
        if (isAutoMoving)
        {
            //사용자의 입력이 들어오면 자동이동 멈춤.
            if (isMoving)
                isAutoMoving = false;


            //position that will be changing
            Vector3 temp = myRectTr.anchoredPosition;
            if (!isOpened)
            {
                if (Mathf.Abs(myRectTr.anchoredPosition.y - max_y) < 0.5f)
                {
                    isAutoMoving = false;
                    temp.y = max_y;
                }
                else
                    temp.y = Mathf.Lerp(myRectTr.anchoredPosition.y, max_y, 0.1f);
            }
            else
            {
                if (Mathf.Abs(myRectTr.anchoredPosition.y - min_y) < 0.5f)
                {
                    isAutoMoving = false;
                    temp.y = min_y;
                }
                else{
                    temp.y = Mathf.Lerp(myRectTr.anchoredPosition.y, min_y, 0.1f);
                    myRectTr.anchoredPosition = temp;
                }
            }
            myRectTr.anchoredPosition = temp;
        }
        else
        {
            UpdateState();
        }
        if (TutorialManager.instance != null && myRectTr.anchoredPosition.y > middle_y)
            TutorialManager.instance.ConditionSatisfied(TutorialManager.ConditionType.BAR_UP);
    }
    void UpdateState()
    {
        isOpened = myRectTr.anchoredPosition.y > min_y;
    }

    public void StartAutoMove()
    {
        if (isAutoMoving)
            isOpened = !isOpened;
        else
            isAutoMoving = true;
    }

    public void Move(Vector2 pos, bool bGroup=false)
    {
        //x값 유지
        Vector2 temp = myRectTr.position;
        temp.y = pos.y;
        myRectTr.position = temp;
    }
    public void MoveEnd(Vector2 pos, bool bGroup = false) { }
}
