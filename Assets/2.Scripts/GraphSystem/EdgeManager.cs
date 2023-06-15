using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EdgeManager : MonoBehaviour
{
    public static EdgeManager instance { get; private set; }

    //엣지 연결정보 - Edge, <outputModule의 Transform, inputModule의 Transform>
    [SerializeField] private Dictionary<NewEdge, KeyValuePair<Transform, Transform>> edge_modulePair;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            edge_modulePair = new Dictionary<NewEdge, KeyValuePair<Transform, Transform>>();
        }
        else Destroy(gameObject);
    }
    public void removeEdge(NewEdge edge, Transform endTr)
    {
        edge_modulePair[edge].Key.SendMessage("RemoveEdge", endTr);
        edge_modulePair.Remove(edge);
        edge.DestroySelf();
    }
    public bool registerEdge(NewEdge edge, Transform endTr, Transform connectionModule)
    {
        //ActivateType input module은 여러개의 output으로부터 연결될수있음
        ActiveInputModule activeInputModule = endTr.GetComponent<ActiveInputModule>();
        if (
            (activeInputModule == null && isAlreadyExistEdge(endTr)) || //Active모듈이 아니면서 이미 연결된 노드이거나
            (endTr.parent == connectionModule.parent)   //동일한 노드에 연결하려한다면
            )
        {
            return false;
        }
        else
        {
            edge_modulePair.Add(edge, new KeyValuePair<Transform, Transform>(connectionModule, endTr));
            return true;
        }
    }
    public bool isAlreadyExistEdge(Transform target)
    {
        foreach (var edgeInfo in edge_modulePair)
            if (edgeInfo.Value.Value.Equals(target))
                return true;
        return false;
    }

    public void onNodeMoving(List<Transform> movingInputmodules, Transform node)
    {
        //모든 연결된 모듈 Pair 순환
        foreach (var output_inputModule in edge_modulePair)
        {
            //output 모듈이 해당 node의 child인지 체크
            if (output_inputModule.Value.Value.IsChildOf(node))
            {
                output_inputModule.Key.LineRendererUpdate();
            }
            //input 모듈이 해당 node의 child인지 체크
            else if (output_inputModule.Value.Key.IsChildOf(node))
            {
                output_inputModule.Key.LineRendererUpdate();
            }
            /*
            //해당 노드의 input module과 같은 inputmodule이 있는지 체크
            foreach (var module in movingInputmodules)
            {
                if (output_inputNode.Value.Key.Equals(module))
                {
                    output_inputNode.Key.LineRendererUpdate();
                    break;
                }
            }*/
        }
    }
    public void UpdateAll()
    {
        foreach (var edgeInfo in edge_modulePair)
        {
            edgeInfo.Key.LineRendererUpdate();
        }
    }
    public void RemoveEdgeFromStringCase(StringCase stringCase)
    {
        StringInputModule inputModule = stringCase.GetComponentInChildren<StringInputModule>();

        foreach (var output_inputModule in edge_modulePair)
        {
            //output module에서 나가는 엣지 제거
            if (output_inputModule.Value.Key.Equals(stringCase.active_output.transform))
            {
                removeEdge(output_inputModule.Key, output_inputModule.Value.Value);
                break;
            }
        }
        foreach (var output_inputModule in edge_modulePair)
        {
            //input module에 연결된 엣지 제거
            if (output_inputModule.Value.Value.Equals(inputModule.transform))
            {
                removeEdge(output_inputModule.Key, output_inputModule.Value.Value);
                break;
            }
        }
    }
    public void RemoveAllEdge()
    {
        List<NewEdge> destroyEdges = new List<NewEdge>();
        Debug.Log("edge_modulePair count : " + edge_modulePair.Count);
        foreach (var pair in edge_modulePair)
        {
            if (pair.Key.gameObject.activeInHierarchy)
            {
                Debug.Log("Removing : " + pair.Value.Value.gameObject.name);
                pair.Value.Key.SendMessage("RemoveEdge", pair.Value.Value.transform);
                pair.Key.DestroySelf();
                destroyEdges.Add(pair.Key);
            }
        }
        foreach (var edge in destroyEdges)
            edge_modulePair.Remove(edge);
    }
}