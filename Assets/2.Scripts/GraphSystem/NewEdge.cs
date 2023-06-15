using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEdge : MonoBehaviour
{
    public static bool AlwaysUpdateLine = false;
    [SerializeField]
    private Transform _startTarget,_endTarget;
    public bool bDestroyed { get; private set; }
    public Transform startTarget
    {
        get { return _startTarget; }
        private set
        {
            _startTarget = value;
            LineRendererUpdate();
        }
    }
    public Transform endTarget
    {
        get { return _endTarget; }
        private set
        {
            _endTarget = value;
            LineRendererUpdate();
        }
    }
    public float directionCoeff;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider2D;
    [SerializeField] private int nPoints;
    private int bHighlighting;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = nPoints;
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        edgeCollider2D.points = new Vector2[nPoints];
        transform.position = new Vector3(0, 0, -10);
        transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);

        SetNormalEdgeSize();
        bDestroyed = false;
    }
    public void Update()
    {
        if (bHighlighting > 0)
            bHighlighting -= 1;
        if (bHighlighting == 1)
            SetNormalEdgeSize();
        if(NewEdge.AlwaysUpdateLine)
            LineRendererUpdate();
    }
    public void UpdatePointSize(int nPoints)
    {
        this.nPoints = nPoints;
    }
    public void LineRendererUpdate()
    {
        lineRenderer.positionCount = nPoints;
        if (startTarget != null && endTarget != null)
        {
            Vector3[] tempArrayVec3 = new Vector3[nPoints];
            Vector2[] tempArrayVec2 = new Vector2[nPoints];
            Vector3 pos;
            Vector3 p0 = startTarget.position;
            Vector3 p1 = endTarget.position;
            directionCoeff = Vector3.Distance(p0, p1) * 1.5f;// Mathf.Min(Vector3.Distance(p0, p1)*1.5f, 20); // 얼마나 양 끝의 미분값을 강하게 할것인지
            Vector3 d0 = startTarget.right * directionCoeff;
            Vector3 d1 = endTarget.right * directionCoeff;

            Vector3 a0 = p0;
            Vector3 a1 = d0;
            Vector3 a2 = 3 * p1 - 3 * p0 - 2 * d0 - d1;
            Vector3 a3 = 2 * p0 - 2 * p1 + d0 + d1;
            float t;
            for (int i = 0; i < nPoints; ++i)
            {
                t = (float)i / (nPoints - 1);
                pos = a0 + a1 * t + a2 * t * t + a3 * t * t * t;
                tempArrayVec3[i] = pos;
                tempArrayVec2[i] = pos;
                //lineRenderer.SetPosition(i, pos);
                //edgeCollider2D.points[i] = pos;
            }
            lineRenderer.SetPositions(tempArrayVec3);
            edgeCollider2D.points = tempArrayVec2;
        }
    }
    public void Highlighting()
    {
        bHighlighting = 3;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
    }
    public void SetNormalEdgeSize()
    {
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
    }
    public void SetStartTarget<T>(OutputModule<T> outputModule)
    {
        startTarget = outputModule.transform;
        lineRenderer.startColor = outputModule.initializedModuleColor;
        lineRenderer.endColor = outputModule.initializedModuleColor;
    }
    public void SetEndTarget(Transform endTr)
    {
        endTarget = endTr;
    }
    static int sortingOrder = 0;
    public void SetOrdering(string sortingLayerName, int sortingOrder)
    {
        lineRenderer.sortingLayerName = "Default";
        lineRenderer.sortingOrder = NewEdge.sortingOrder++;
        //Debug.Log("edge sortingOrder = " + NewEdge.sortingOrder);
        //lineRenderer.sortingLayerName = sortingLayerName;
        //lineRenderer.sortingOrder = sortingOrder;
    }
    //Destory호출된 상태에서 접근하지못하기위해 별도의 메소드로 삭제
    public void DestroySelf()
    {
        if (!bDestroyed)
        {
            bDestroyed = true;
            Destroy(gameObject);
            Debug.Log("Edge  Destroy Self called.");
        }
    }
    private void OnDestroy()
    {
        if (!bDestroyed)
            Debug.LogWarning("Edge was destroyed by illegal way");
        bDestroyed = true;
    }
}
