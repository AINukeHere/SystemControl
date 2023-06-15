using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 노드들을 정렬하여 겹쳤을때 이상하게 보이지 않도록 합니다.
/// Folder, Node
/// </summary>
public class SortingNodes : Editor
{
    [MenuItem("Tool/SortingNodes")]
    static void Init()
    {
        int nodeOrder = 0;
        int folderOrder = 0;
        Folder[] folders = FindObjectsOfType<Folder>();
        foreach(Folder folder in folders)
        {
            Debug.Log("folder idx = " + folderOrder);
            folder.gameObject.layer = LayerMask.NameToLayer("Graph");
            SpriteRenderer spriteRenderer = folder.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Folder";
            spriteRenderer.sortingOrder = folderOrder++;

            Debug.Log("folder's textmesh idx = " + folderOrder);
            TextMesh textMesh = folder.GetComponentInChildren<TextMesh>();
            textMesh.gameObject.layer = LayerMask.NameToLayer("Graph");
            MeshRenderer meshRenderer = textMesh.GetComponent<MeshRenderer>();
            meshRenderer.sortingLayerName = "Folder";
            meshRenderer.sortingOrder = folderOrder++;

        }

        Node[] nodes = FindObjectsOfType<Node>();
        foreach(Node node in nodes)
        {
            SpriteRenderer[] spriteRenderers = node.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.sortingLayerName = "Node";
                spriteRenderer.sortingOrder = ++nodeOrder;
                Debug.Log(node.gameObject.name + "'s " + spriteRenderer.gameObject.name + " = " + nodeOrder);
            }

            // 스프라이트 처리시 상수노드의 경우 마스킹 문제로 order는 node sprite < node mask < edgePoint sprite < edgePoint mask가 되어야함.
            if (node is GetValue)
            {
                SpriteMask[] spriteMasks = node.GetComponentsInChildren<SpriteMask>();
                List<SpriteRenderer> spriteRenderersInMasksChildren = new List<SpriteRenderer>();
                foreach (SpriteMask spriteMask in spriteMasks)
                {
                    spriteMask.isCustomRangeActive = true;
                    spriteMask.frontSortingLayerID = SortingLayer.NameToID("Node");
                    spriteMask.frontSortingOrder = ++nodeOrder;
                    Debug.Log(node.gameObject.name + "'s " + spriteMask.gameObject.name + " = " + nodeOrder);

                    var masks = spriteMask.GetComponentsInChildren<SpriteRenderer>();
                    foreach(var mask in masks)
                    {
                        if (mask.gameObject.Equals(spriteMask.gameObject))
                            continue;
                        spriteRenderersInMasksChildren.Add(mask);
                    }
                }
                foreach (SpriteRenderer spriteRenderer in spriteRenderersInMasksChildren)
                {
                    spriteRenderer.sortingLayerName = "Node";
                    spriteRenderer.sortingOrder = ++nodeOrder;
                    Debug.Log(node.gameObject.name + "'s " + spriteRenderer.gameObject.name + " = " + nodeOrder);

                    SpriteMask[] spriteMasksInSprite = spriteRenderer.GetComponentsInChildren<SpriteMask>();
                    foreach (SpriteMask spriteMask in spriteMasksInSprite)
                    {
                        spriteMask.isCustomRangeActive = false;
                    }
                }
            }
            else
            {
                SpriteMask[] spriteMasks = node.GetComponentsInChildren<SpriteMask>();
                foreach (SpriteMask spriteMask in spriteMasks)
                {
                    spriteMask.isCustomRangeActive = true;
                    spriteMask.frontSortingLayerID = SortingLayer.NameToID("Node");
                    spriteMask.frontSortingOrder = ++nodeOrder;
                    Debug.Log(node.gameObject.name + "'s " + spriteMask.gameObject.name + " = " + nodeOrder);
                }
            }

            MeshRenderer[] meshRenderers = node.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.sortingLayerName = "Node";
                meshRenderer.sortingOrder = ++nodeOrder;
                Debug.Log(node.gameObject.name + "'s " + meshRenderer.gameObject.name + " = " + nodeOrder);
            }
            Transform[] transforms = node.GetComponentsInChildren<Transform>();
            foreach (Transform tr in transforms)
            {
                tr.gameObject.layer = LayerMask.NameToLayer("Graph");
            }
        }
        //TextMesh[] objects = GameObject.FindObjectsOfType<TextMesh>();
        //foreach (TextMesh obj in objects)
        //{
        //    MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        //    if (obj.transform.parent.GetComponent<Folder>() != null)
        //    {
        //        renderer.sortingLayerName = "Folder";
        //        renderer.sortingOrder = folderIdx++;
        //        Debug.Log(obj.transform.parent.name + " is Folder");
        //    }
        //    else
        //    {
        //        renderer.sortingLayerName = "Default";
        //        renderer.sortingOrder = i;
        //        Debug.Log(obj.transform.parent.name + " is Default");
        //        i += 3;
        //    }
        //}
        //i = 0;
        //Node[] nodeObjects = GameObject.FindObjectsOfType<Node>();
        //foreach (Node obj in nodeObjects)
        //{
        //    SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        //    renderer.sortingLayerName = "Default";
        //    renderer.sortingOrder = i;
        //    Debug.Log(obj.name + " is Default");
        //    i += 3;
        //}
        //i = 0;
        //GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("EdgePoint");
        //foreach (GameObject edgePoint in gameobjects)
        //{
        //    SpriteRenderer[] renderer = edgePoint.GetComponentsInChildren<SpriteRenderer>();

        //    renderer[0].sortingLayerName = "Default";
        //    renderer[0].sortingOrder = i++;
        //    renderer[1].sortingLayerName = "Default";
        //    renderer[1].sortingOrder = i++;
        //    Debug.Log(edgePoint.name + " is Default");
        //}
    }
}