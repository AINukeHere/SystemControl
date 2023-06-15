using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class BluePrint : EditorWindow
{
    Camera camera;
    RenderTexture renderTexture;

    [MenuItem("Window/Blue Print Editor")]
    static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(BluePrint));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
    }

    public void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("BluePrintCamera").GetComponent<Camera>();
        renderTexture = new RenderTexture((int)position.width,
                (int)position.height,
                (int)RenderTextureFormat.ARGB32);
        addNodePopupRect = new Rect(position.xMax, position.yMax, 200, 300);
    }

    public void Update()
    {
        if (camera != null)
        {
            camera.targetTexture = renderTexture;
            camera.Render();
            camera.targetTexture = null;
        }
        if (renderTexture.width != position.width ||
            renderTexture.height != position.height)
            renderTexture = new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
    }

    public Rect addNodePopupRect;
    public string node_name_input;
    public Vector2 scrollPos = Vector2.zero, scrollPos2 = Vector2.zero;
    void OnGUI()
    {
        //씬드로우
        GUI.DrawTexture(new Rect(0.0f, 0.0f, position.width, position.height), renderTexture);

        //텍스트필드
        Rect textFiledRect = addNodePopupRect; textFiledRect.height = 20;
        node_name_input = GUI.TextField(textFiledRect, node_name_input);

        //요소 생성준비
        //전체경로
        string[] paths = DirSearch(Application.dataPath + "/3.Prefabs/Node/", "*.prefab");
        //정렬된 리스트 (파일이름, 전체경로)
        SortedList<string, string> sorted_list = new SortedList<string, string>();
        for (int i = 0; i < paths.Length; ++i)
        {
            string temp = paths[i].Replace(Application.dataPath, "").Replace('\\', '/');
            sorted_list.Add(Path.GetFileName(temp), temp);
        }

        List<string> result = new List<string>(sorted_list.Keys);
        List<string> found_result;
        if (string.IsNullOrEmpty(node_name_input))
            found_result = result;
        else
            found_result = result.FindAll(s => s.ToLower().Contains(node_name_input.ToLower()));

        //스크롤뷰시작
        Rect scrollRect = new Rect(addNodePopupRect.position, new Vector2(220, Mathf.Clamp(found_result.Count * 20,0,300))); scrollRect.y += 20;
        scrollPos = GUI.BeginScrollView(scrollRect, scrollPos, new Rect(0, 0, 220, found_result.Count * 20));


        //버튼 텍스트앵커 조절
        TextAnchor oldAnchor = GUI.skin.button.alignment;
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;

        //버튼 생성
        for (int i = 0; i < found_result.Count; ++i)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets" + sorted_list[found_result[i]], typeof(GameObject));
            if (prefab != null)
            {
                if (GUI.Button(new Rect(0, i * 20, 200, 20), prefab.name))
                {
                    Vector3 bj = addNodePopupRect.position;
                    bj.y = position.height - bj.y;
                    Vector2 coordSys = camera.ScreenToWorldPoint(bj);
                    Vector3 pos = coordSys;
                    pos.z = 0;
                    Instantiate(prefab, pos, Quaternion.identity);
                    addNodePopupRect.position = new Vector2(position.xMax, position.yMax);
                    node_name_input = "";
                }
            }
            else
                GUI.Button(new Rect(0, i * 20, 200, 20), "not prefab");
        }
        GUI.skin.button.alignment = oldAnchor;

        //스크롤뷰 끝
        GUI.EndScrollView();

        //마우스우클릭
        UnityEngine.Event e = UnityEngine.Event.current;
        if (e.type == EventType.MouseDown)
        {
            if (e.button == 1)
                addNodePopupRect.position = e.mousePosition;
            else if (e.button == 0)
            {
                addNodePopupRect.position = new Vector2(position.xMax, position.yMax);
                node_name_input = "";
            }
        }
    }
    void OnDestroy()
    {
        Debug.Log("OnDestroy()");
    }

    static string[] DirSearch(string sDir, string searchPattern)
    {
        List<string> paths = new List<string>();
        try
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d, searchPattern))
                {
                    paths.Add(f);
                }
                DirSearch(d, searchPattern);
            }
        }
        catch (System.Exception excpt)
        {
            Debug.LogError(excpt.Message);
        }
        return paths.ToArray();
    }
}