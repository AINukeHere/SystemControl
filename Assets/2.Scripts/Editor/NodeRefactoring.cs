using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 노드들을 정렬하여 겹쳤을때 이상하게 보이지 않도록 합니다.
/// Folder, Node
/// </summary>
public class NodeRefactoring : Editor
{
    [MenuItem("Tool/NodeRefactoring")]
    static void Init()
    {
        //resources

        Node[] nodes = FindObjectsOfType<Node>();
        NodeManager mgr = NodeManager.instance;
        //Font defaultFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        Font defaultFont = Resources.Load<Font>("NanumGothicBold");
        if (mgr == null)
        {
            mgr = FindObjectOfType<NodeManager>();
            mgr.InitializeNodeInfo();
        }

        foreach (Node node in nodes)
        {
            var trs = node.GetComponentsInChildren<Transform>();

            GameObject nodeTitleGO = null;
            SpriteRenderer nodeTitleWhiteSquare = null;
            GameObject nodeTitleTextMeshGO = null;
            TextMesh nodeTitleTextMesh = null;

            foreach (var tr in trs)
            {
                if (tr.gameObject.name == "NodeTitle")
                {
                    nodeTitleGO = tr.gameObject;
                }
            }


            // 노드 타이틀
            if (node is GetValue) // 값을 가져오는 상수노드, GetAvoiderVelocity 등은 따로 타이틀이 없이 노드 몸체가 곧 타이틀이다.
            {
                nodeTitleTextMesh = node.GetComponentInChildren<TextMesh>();
            }
            else // 그 외에는 흰색배경의 노드 타이틀을 가진다.
            {
                if (nodeTitleGO == null)
                {
                    nodeTitleGO = new GameObject("NodeTitle");
                    nodeTitleGO.transform.parent = node.transform;
                }
                nodeTitleWhiteSquare = nodeTitleGO.GetComponent<SpriteRenderer>();
                if (nodeTitleWhiteSquare == null)
                {
                    nodeTitleWhiteSquare = nodeTitleGO.AddComponent<SpriteRenderer>();
                }
                nodeTitleWhiteSquare.sprite = Resources.Load("white_square", typeof(Sprite)) as Sprite;
                Vector3 nodeScale = node.transform.lossyScale;
                nodeTitleGO.transform.localScale = new Vector3(1, 0.6f / nodeScale.y, 1);
                nodeTitleGO.transform.localPosition = new Vector3(0, (1 + (0.6f / nodeScale.y)), 0);

                nodeTitleTextMesh = nodeTitleGO.GetComponentInChildren<TextMesh>();
                if (nodeTitleTextMesh == null)
                {
                    nodeTitleTextMeshGO = new GameObject("TextMesh");
                    nodeTitleTextMeshGO.transform.parent = nodeTitleGO.transform;
                    nodeTitleTextMesh = nodeTitleTextMeshGO.AddComponent<TextMesh>();
                }
                else
                    nodeTitleTextMeshGO = nodeTitleTextMesh.gameObject;
                nodeTitleTextMeshGO.transform.localPosition = Vector3.zero;
                Vector3 nodeTitleGScale = nodeTitleTextMeshGO.transform.parent.lossyScale;
                nodeTitleTextMeshGO.transform.localScale = new Vector3(
                    1 / nodeTitleGScale.x,
                    1 / nodeTitleGScale.y,
                    1 / nodeTitleGScale.z);
                nodeTitleTextMesh.font = defaultFont;
                nodeTitleTextMeshGO.GetComponent<MeshRenderer>().material = defaultFont.material;
                nodeTitleTextMesh.fontSize = 10;
                nodeTitleTextMesh.offsetZ = -1;
                nodeTitleTextMesh.color = Color.black;
                nodeTitleTextMesh.text = mgr.getNodeName(node.GetType());
                nodeTitleTextMesh.alignment = TextAlignment.Center;
                nodeTitleTextMesh.anchor = TextAnchor.MiddleCenter;
            }

            
            //배경색을 고려하여 노드 내부 테스트컬러를 흰색 혹은 검은색으로 결정
            Color colorCodeInNodeBody = new Color();
            Color colorOfNodeBody = node.GetComponent<SpriteRenderer>().color;
            float r, g, b, L;
            r = colorOfNodeBody.r;
            g = colorOfNodeBody.g;
            b = colorOfNodeBody.b;
            r = r <= 0.04045 ? r : Mathf.Pow((r + 0.055f) / 1.055f, 2.4f);
            g = g <= 0.04045 ? g : Mathf.Pow((g + 0.055f) / 1.055f, 2.4f);
            b = b <= 0.04045 ? b : Mathf.Pow((b + 0.055f) / 1.055f, 2.4f);
            L = 0.2126f * r + 0.7152f * g + 0.0722f * b;
            colorCodeInNodeBody = L > 0.179 ? Color.black : Color.white;

            if (node is GetValue)
            {
                ModuleColorizable module = node.GetComponentInChildren<ModuleColorizable>();
                Vector3 nodeGScale = node.transform.lossyScale;
                Vector3 moduleTargetScale = node is ConstantNode ? Vector3.one : new Vector3(0.4f, 1, 1);
                module.transform.localScale = new Vector3(
                    moduleTargetScale.x / nodeGScale.x,
                    moduleTargetScale.y / nodeGScale.y,
                    moduleTargetScale.z / nodeGScale.z);

                TextMesh textMesh = module.GetComponentInChildren<TextMesh>();
                Vector3 moduleGScale = module.transform.lossyScale;
                textMesh.transform.localScale = new Vector3(1 / moduleGScale.x, 1 / moduleGScale.y, 1 / moduleGScale.z);
                textMesh.transform.localPosition = new Vector3(-module.transform.localPosition.x / module.transform.localScale.x, 0, 0);
                textMesh.alignment = TextAlignment.Center;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.font = defaultFont;
                textMesh.GetComponent<MeshRenderer>().material = defaultFont.material;
                textMesh.fontSize = node is ConstantNode ? 13 : 10;
                textMesh.offsetZ = -1;
                textMesh.color = colorCodeInNodeBody;
                // ConstantNode면 자료형을, 일반 GetVariable이라면 GetVariable형태로 설정
                textMesh.text = node is ConstantNode ? mgr.getModuleType(module.GetType()) : mgr.getNodeName(node.GetType());
                module.RegisterTextMesh(textMesh);
            }
            else
            {
                ModuleColorizable[] all_modules = node.GetComponentsInChildren<ModuleColorizable>();
                int numInputModule = 0;
                int numOutputModule = 0;
                foreach (ModuleColorizable module in all_modules)
                {
                    string moduleTypeName = module.GetType().ToString();
                    if (moduleTypeName.EndsWith("OutputModule"))
                        numOutputModule++;
                    else if (moduleTypeName.EndsWith("InputModule"))
                        numInputModule++;
                    else
                        Debug.LogError($"{node.gameObject.name}의 {module.GetType().ToString()}이 OutputModule이나 InputModule로 끝나지 않았습니다.");
                }
                foreach (ModuleColorizable module in all_modules)
                {
                    Vector3 moduleScale = new Vector3();
                    bool bInputModule = false;
                    string moduleTypeName = module.GetType().ToString();
                    Vector3 nodeGScale = module.transform.parent.lossyScale;
                    if (moduleTypeName.EndsWith("OutputModule"))
                    {
                        bInputModule = false;
                        moduleScale = numOutputModule > 1 ? new Vector3(0.4f, 0.8f, 1) : new Vector3(0.4f, nodeGScale.y, 1);
                    }
                    else if (moduleTypeName.EndsWith("InputModule"))
                    {
                        bInputModule = true;
                        moduleScale = numInputModule > 1 ? new Vector3(0.4f, 0.8f, 1) : new Vector3(0.4f, nodeGScale.y, 1);
                    }
                    else
                        Debug.LogError($"{node.gameObject.name}의 {module.GetType().ToString()}이 OutputModule이나 InputModule로 끝나지 않았습니다.");

                    module.transform.localScale = new Vector3(
                        moduleScale.x / nodeGScale.x,
                        moduleScale.y / nodeGScale.y,
                        moduleScale.z / nodeGScale.y);

                    GameObject textMeshGameObject = null;
                    textMeshGameObject = module.transform.Find("textMesh")?.gameObject;
                    if (textMeshGameObject == null)
                    {
                        textMeshGameObject = new GameObject("textMesh");
                        textMeshGameObject.transform.parent = module.transform;
                    }
                    textMeshGameObject.transform.localPosition = bInputModule ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);

                    Vector3 pScale = textMeshGameObject.transform.parent.lossyScale;
                    textMeshGameObject.transform.localScale = new Vector3(1 / pScale.x, 1 / pScale.y, 1 / pScale.z);

                    TextMesh textMesh = null;
                    textMesh = textMeshGameObject.GetComponent<TextMesh>();
                    if (textMesh == null)
                    {
                        textMesh = textMeshGameObject.AddComponent<TextMesh>();
                    }
                    textMesh.alignment = TextAlignment.Left;
                    textMesh.anchor = bInputModule ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
                    textMesh.font = defaultFont;
                    textMesh.GetComponent<MeshRenderer>().material = defaultFont.material;
                    textMesh.fontSize = 10;
                    textMesh.offsetZ = -1;
                    textMesh.color = colorCodeInNodeBody;
                    textMesh.text = mgr.getModuleType(module.GetType());
                }
            }
            node.RegisterBaseTextMeshes(nodeTitleTextMesh);
        }
    }
}