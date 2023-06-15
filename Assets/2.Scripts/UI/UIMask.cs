using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMask : MonoBehaviour, IMovable
{


    RectTransform myRectTr;
    public RectTransform bottom_line;
    public Canvas canvas;
    float deltaY;
    public float anchoredX, anchoredY, aspect;
    void Awake()
    {
        myRectTr = GetComponent<RectTransform>();
        //고정된 x값 1920
        anchoredX = canvas.GetComponent<CanvasScaler>().referenceResolution.x;
        //종횡비
        aspect = canvas.pixelRect.yMax / canvas.pixelRect.xMax;
        //가로는 1920 고정 세로는 종횡비로 계산
        anchoredY = anchoredX * aspect;
        GetComponent<BoxCollider2D>().size = new Vector2(anchoredX, anchoredY);


        deltaY = myRectTr.anchoredPosition.y - bottom_line.anchoredPosition.y;
    }
    void Update()
    {
        myRectTr.anchoredPosition = bottom_line.anchoredPosition + new Vector2(0, deltaY);
    }
    //Movable
    public bool isMoving
    {
        get;
        set;
    }
    public void Move(Vector2 pos, bool bGroup=false)
    {
        //x값 유지
        Vector2 temp = myRectTr.position;
        temp.y = pos.y;
        myRectTr.position = temp;
        bottom_line.anchoredPosition = myRectTr.anchoredPosition - new Vector2(0, deltaY);
        bottom_line.GetComponent<PanelMove>().isAutoMoving = false;
    }
    public void MoveEnd(Vector2 pos, bool bGroup = false) { }
}
