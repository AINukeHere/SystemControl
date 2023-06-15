using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//모듈의 색깔을 색칠하는 툴을 만들었으나 Nodemanager의 instance가 없는 등 문제가 있어서 걍 버림 게임 시작할때 적용됨 ㅅㄱ
//public class ModuleColorizeToolMenu : Editor
//{
//    [MenuItem("Tool/ModuleColorize")]
//    static void Init()
//    {
//        ModuleColorize[] mcs = FindObjectsOfType<ModuleColorize>();
//        foreach(var mc in mcs)
//        {
//            mc.Colorize();
//        }
//    }
//}
public class EdgePointLocalPositionSetZero : Editor
{
    [MenuItem("Tool/EdgePointLocalPositionSetZero")]
    static void Init()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EdgePoint");
        foreach (GameObject obj in objects)
        {
            obj.name = "EdgePoint";
            obj.transform.localPosition = new Vector3(0, 0, 0);
            if (obj.transform.parent == null)
            {
                obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
            }
            else
            {
                Transform parent = obj.transform.parent;
                obj.transform.localScale = new Vector3(
                    0.4f / parent.lossyScale.x,
                    0.4f / parent.lossyScale.y,
                    1 / parent.lossyScale.z);
            }
        }
    }
}
public class DotNamingWithObjectName : Editor
{
    [MenuItem("Tool/DotRename")]
    static void Init()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Dot");
        foreach (GameObject obj in objects)
        {
            char[] num = new char[obj.name.Length - 3];
            obj.name.CopyTo(3, num, 0, obj.name.Length - 3);
            obj.GetComponentInChildren<TextMesh>().text = new string(num);
        }
    }
}
public class UI_Rendering_Order_Sort : Editor
{
    [MenuItem("Tool/UI_Sort")]
    static void Init()
    {
        Canvas[] canvasList = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasList)
        {
            UI_Render_Order_Setting[] uI_Rendering_Order_Sorts = canvas.GetComponentsInChildren<UI_Render_Order_Setting>(true);
            Debug.Log("UI length : " + uI_Rendering_Order_Sorts.Length);
            foreach (UI_Render_Order_Setting ui in uI_Rendering_Order_Sorts)
            {
                ui.Sorting();
            }
        }
    }
}