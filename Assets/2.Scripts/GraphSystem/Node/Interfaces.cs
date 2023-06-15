using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

//위치,방향이 리셋가능한 인터페이스입니다.
public interface IResetable
{
    Vector3 origin_pos
    {
        get; set;
    }
    Quaternion origin_rot
    {
        get; set;
    }
    void Awake();
    void ResetNode();
}
//설명을 보여줄수 있는 인터페이스입니다.
public interface IHasInfo
{
    string GetInfoString();
}
//눈 아이콘을 눌러서 세부적인 정보를 출력할 수 있는 인퍼에시으입니다.
public interface IExpandableDisplay
{
    bool isExpanded
    {
        get; set;
    }
}
//마우스로 끌어서 움직일 수 있는 인터페이스입니다.
public interface IMovable
{
    bool isMoving
    {
        get; set;
    }
    void Move(Vector2 pos, bool bGroup = false);
    void MoveEnd(Vector2 pos, bool bGroup = false);
}
//NewEdge가 이것에 연결됩니다. 모든 InputModule이 상속받습니다.
public interface IInputParam<T>
{
    void Input(T input);
}

//모든 Module이 상속받습니다.
[RequireComponent(typeof(SpriteRenderer))]
public abstract class ModuleColorizable : MonoBehaviour, IExpandableDisplay
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private ModuleValueType valueType_for_colorize;
    [SerializeField] protected TextMesh textMesh;
    protected string inputNormalDisplayString;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Colorize();
        inputNormalDisplayString = NodeManager.instance.getModuleType(GetType());
    }
    public virtual void OnRenderObject()
    {
        UpdateDisplay();
    }
    public virtual void UpdateDisplay()
    {
        if (isExpanded)
            ExpandDisplay();
        else
            NormalDisplay();
    }
    public void Colorize()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = NodeManager.instance.getModuleTypeColor(valueType_for_colorize);
    }
    public void RegisterTextMesh(TextMesh textMesh)
    {
        if (Application.isPlaying)
        {
            Debug.LogError("cannot register");
            return;
        }
        this.textMesh = textMesh;
    }

    public bool isExpanded { get; set; }
    public virtual void NormalDisplay()
    {
        textMesh.text = inputNormalDisplayString;
    }
    public abstract void ExpandDisplay();
}
//모든 노드가 상속받는 클래스입니다.
public abstract class Node : MonoBehaviour, IResetable, IHasInfo, IMovable
{
    [SerializeField] protected TextMesh nodeTitle;
    protected string nodeName;
    public Vector3 origin_pos{get; set;}
    public Quaternion origin_rot{get; set;}

    //input, output module의 transform for update to connected Edge
    protected List<Transform> allInputModules;
    public virtual void Awake()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("Node Awake()");
#endif
        nodeName = NodeManager.instance.getNodeName(GetType());
        origin_pos = transform.position;
        origin_rot = transform.rotation;
        allInputModules = new List<Transform>();
        CheckAllModules();
        nodeTitle.text = nodeName;
    }
    public virtual void CheckAllModules()
    {
        var trs = GetComponentsInChildren<Transform>();
        foreach(var tr in trs)
        {
            //InputMoudles
            ModuleColorizable module = GetComponent<ModuleColorizable>();
            if (module && module.GetType().ToString().EndsWith("InputModule"))
                allInputModules.Add(tr);
        }
    }
    
    public virtual void ResetNode()
    {
        //부모가 없을 때만 리셋 (폴더아래에 있을땐 무반응)
        if (transform.parent == null)
        {
            transform.position = origin_pos;
            transform.rotation = origin_rot;
        }
    }

    public abstract void CheckOutput();
    public string GetInfoString()
    {
        return NodeManager.instance.getExplainText(this.GetType());
    }

    //Movable 인터페이스 구현
    public bool isMoving { get; set; }

    public virtual void Move(Vector2 pos, bool bGroup = false)
    {
        Collider2D[] colls = Physics2D.OverlapAreaAll((Vector3)pos - transform.lossyScale, (Vector3)pos + transform.lossyScale);
        int i;
        for (i = 0; i < colls.Length; ++i)
            if (colls[i].CompareTag("LockField"))
                break;
        if (i == colls.Length)
            transform.position = pos;
        EdgeManager.instance.onNodeMoving(allInputModules, transform);
    }
    public virtual void MoveEnd(Vector2 pos, bool bGroup = false) { }

    /// <summary>
    /// IMPORTANT: Must call by NodeRefactoring.
    /// NOTICE: this method execute only Game is not Playing.
    /// </summary>
    /// <param name="nodeTitle">textMesh for nodeTitle</param>
    public void RegisterBaseTextMeshes(TextMesh nodeTitle)
    {
        if (Application.isPlaying)
            return;
        this.nodeTitle = nodeTitle;
    }
}
//활성화가능한(실행 가능한) 노드클래스 = Function, Control, SetVariable
public abstract class ActivatableNode : Node
{
    public int isActive;
    protected SpriteRenderer myRenderer;
    public virtual void Active()
    {
        isActive = 2;
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.NodeActivated(gameObject.name);
        }
        CheckOutput();
    }
    public override void Awake()
    {
        base.Awake();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnRenderObject()
    {
        UpdateDisplay();
    }

    public virtual void UpdateDisplay()
    {
        Color temp = myRenderer.color;
        if (isActive >= 1)
        {
            temp.a = 1;
            isActive--;
        }
        else
            temp.a = 0.5f;
        myRenderer.color = temp;
    }

}
//연산자 노드가 아닌 실행신호가 없는 노드클래스 = Object에 대한 Method node (ex. isPlayingAudioSource, PlayAudioSource)
public abstract class MethodNode : Node
{
    protected SpriteRenderer myRenderer;
    public abstract bool CheckRuningState();
    public override void Awake()
    {
        base.Awake();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnRenderObject()
    {
        UpdateDisplay();
    }
    public virtual void UpdateDisplay()
    {
        Color temp = myRenderer.color;
        if (CheckRuningState())
            temp.a = 1;
        else
            temp.a = 0.5f;
        myRenderer.color = temp;
    }

}
//ArrowOutput이 Active시키지 않고 노드가 아닌 활성화클래스 (SwitchCase, Folder)
public class NotInputNotNodeActivatable : MonoBehaviour
{
    public int isActive;

    protected SpriteRenderer myRenderer;
    public virtual void Active()
    {
        isActive = 2;
    }
    public virtual void Awake()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void OnRenderObject()
    {
        UpdateDisplay();
    }
    public virtual void UpdateDisplay()
    {
        Color temp = myRenderer.color;
        if (isActive >= 1)
        {
            isActive--;
            temp.a = 1;
        }
        else
            temp.a = 0.5f;
        myRenderer.color = temp;
    }
}
//모든 연산자 노드 부모
/// <summary>
/// T : 인풋타입 R : 결과타입
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="R"></typeparam>
public abstract class Operator<T, R> : Node
{
    //인풋모듈에서 어싸인
    public int input_size = 0, result_size = 0;
    protected T[] input;
    protected R[] output;

    //Awake에서 어싸인
    protected OutputModule<R>[] outputModules;


    public override void Awake()
    {
        base.Awake();
        input = new T[input_size];
        output = new R[result_size];
        outputModules = GetComponentsInChildren<OutputModule<R>>();
        if (outputModules == null)
            Debug.LogError("RelationalOperator can't find output module");
    }
    public virtual void Update()
    {
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("Operator default(T)");
        for (int i = 0; i < input.Length; ++i)
        {
            input[i] = default;
        }
        for (int i = 0; i < output.Length; ++i)
        {
            output[i] = default;
        }
    }

    public void Input(T input, int idx)
    {
        if (input != null)
        {
            if (gameObject.name.EndsWith("(Test)"))
                Debug.Log("Operator CheckOutput() : " + input.ToString());
            this.input[idx] = input;
            CheckOutput();
        }
    }
}

/*
InputModule 부모는 만들 수 없음. Generic Programming Seriallize 지원안함 ParamEvent<T>가 직렬화 될 수 없음.
[System.Serializable]
public class ParamEvent<T> : UnityEvent<T, int> { }
public abstract class InputModule<T> : MonoBehaviour, InputParam<T>
{
    [SerializeField]
    public ParamEvent<T> destination;
    [SerializeField]
    protected int input_index;
    public abstract void Input(T input);
}
*/

//InputModule 부모
public abstract class InputModule<T> : ModuleColorizable, IInputParam<T>
{
    protected T input_for_visualize;
    public virtual void Input(T input)
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log($"InputModule input_for_visualize = input {input}");
#endif
        input_for_visualize = input;
    }
    public virtual void Update()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("InputModule input_for_visualize = default");
#endif
        ResetInputForVisualize();
    }
    public virtual void ResetInputForVisualize()
    {
        input_for_visualize = default;
    }
    public override void ExpandDisplay()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("InputModule input_for_visualize = default");
#endif
        textMesh.text = input_for_visualize != null ? input_for_visualize.ToString() : "값없음";
    }
}
/// <summary>
/// OutputModule 부모
/// ActivateTypeModule의 경우 base로 부모메소드를 호출하지않으므로 각별히 관리필요
/// </summary>
public abstract class OutputModule<T> : ModuleColorizable, IInputParam<T>, IMovable
{
    private GameObject edgePrefab;
    private List<NewEdge> edges;
    private NewEdge currentEdge;
    [SerializeField]
    private static float autoAssignRadius = 0.5f;
    [SerializeField] protected List<IInputParam<T>> connectedInputModules = new List<IInputParam<T>>();
    [Tooltip("미리 연결되어있도록 설정합니다. InputModule의 Transform을 등록")]
    [SerializeField] private Transform[] defaultEdge;
    public Color initializedModuleColor;
    protected T input;

    protected override void Awake()
    {
        base.Awake();
        edges = new List<NewEdge>();
        edgePrefab = Resources.Load<GameObject>("NewEdge");
        initializedModuleColor = GetComponentInParent<SpriteRenderer>().color;
        CreateDefaultEdges();
    }
    public virtual void Update()
    {
        ResetInput();
    }
    public virtual void ResetInput()
    {
        if (typeof(T) == typeof(ActivateType))
            Debug.LogError("RESETINPUT");
        input = default;
    }
    public virtual void Input(T input)
    {
        this.input = input;
        if (input != null)
            CheckOutput();
    }
    public void CheckOutput()
    {
        try
        {
            foreach (IInputParam<T> connectedInputModule in connectedInputModules)
            {
#if UNITY_EDITOR
                if (gameObject.name.EndsWith("(Test)"))
                    Debug.Log(connectedInputModule.ToString());
#endif
                connectedInputModule.Input(input);
            }
        }
        catch(NullReferenceException e)
        {
            Debug.Log(gameObject.name);
            Debug.LogError(e);
        }
        AfterInputCallBack();
    }
    public bool isMoving { get; set; }
    public virtual void MoveBegin()
    {
        currentEdge = Instantiate(edgePrefab).GetComponent<NewEdge>();
        edges.Add(currentEdge);
        currentEdge.SetStartTarget(this);
        currentEdge.SetEndTarget(GameObject.Find("Controller").transform);

        //Ordering
        SpriteRenderer edgePointRenderer = GetComponentInChildren<SpriteRenderer>();
        string sortingLayerName = edgePointRenderer.sortingLayerName;
        int sortingOrder = edgePointRenderer.sortingOrder;
        currentEdge.SetOrdering(sortingLayerName, sortingOrder);
    }
    public virtual void Move(Vector2 pos, bool bGroup = false)
    {
        if (bGroup)
            return;
        if (!isMoving)
        {
            if (typeof(T) != typeof(ActivateType) || edges.Count == 0) //실행신호를 전달하는 모듈인경우에는 둘 이상의 연결선이 존재할 수 없다.
            {
                MoveBegin();
                isMoving = true;
            }
        }
        if (currentEdge != null)
            currentEdge.LineRendererUpdate();
    }
    public virtual void MoveEnd(Vector2 pos, bool bGroup=false)
    {
        if (!isMoving || bGroup)
            return;
        bool bFoundInputModule = false;  //colls에서 InputNode를 찾았는지 체크
        Collider2D[] colls;
        colls = Physics2D.OverlapCircleAll(pos, autoAssignRadius);
        Debug.Log(colls.Length);
        if (colls.Length > 0)
        {
            int min_dist_idx = -1;
            for (int i = 0; i < colls.Length; ++i)
            {
                Collider2D coll = colls[i];
                if (coll.CompareTag("InputNode"))
                {
                    //아직 최저가 결정되지 않았거나 최저보다 더 적을 때 갱신
                    if (min_dist_idx == -1 ||
                        Vector3.Distance(coll.transform.position, pos) < Vector3.Distance(colls[min_dist_idx].transform.position, pos))
                        min_dist_idx = i;
                }
            }
            if (min_dist_idx != -1)
            {
                IInputParam<T> nearestInputModule = colls[min_dist_idx].GetComponent<IInputParam<T>>();
                if (nearestInputModule != null)
                {
                    bool bExist = false;
                    foreach (IInputParam<T> target in connectedInputModules)
                    {
                        if (target.Equals(nearestInputModule))
                        {
                            bExist = true;
                            break;
                        }
                    }
                    if (!bExist)
                    {
                        if (EdgeManager.instance.registerEdge(currentEdge, colls[min_dist_idx].transform, transform))
                        {
                            connectedInputModules.Add(nearestInputModule);
                            currentEdge.SetEndTarget(colls[min_dist_idx].transform);
                            Debug.Log(colls[min_dist_idx].gameObject.name + " connected");
                            bFoundInputModule = true;
                        }
                        else
                            Debug.LogWarning("Edge Register Failed : " + name + " -> " + colls[min_dist_idx].gameObject.name);
                    }
                }
            }
        }
        if (!bFoundInputModule)
        {
            edges.Remove(currentEdge);
            currentEdge.DestroySelf();
            currentEdge = null;
        }

        isMoving = false;
    }
    public virtual void CreateEdge(Transform targetTr)
    {
        currentEdge = Instantiate(edgePrefab).GetComponent<NewEdge>();
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("DEBUG");
#endif
        if (EdgeManager.instance.registerEdge(currentEdge, targetTr, transform))
        {
            IInputParam<T> targetInputModule = targetTr.GetComponent<IInputParam<T>>();
            edges.Add(currentEdge);
            connectedInputModules.Add(targetInputModule);
            currentEdge.SetStartTarget(this);
            currentEdge.SetEndTarget(targetTr);

            //Ordering
            SpriteRenderer edgePointRenderer = GetComponentInChildren<SpriteRenderer>();
            string sortingLayerName = edgePointRenderer.sortingLayerName;
            int sortingOrder = edgePointRenderer.sortingOrder;
            currentEdge.SetOrdering(sortingLayerName, sortingOrder);
        }
        else
        {
            Debug.LogWarning("Edge Register Failed : " + name + " -> " + targetTr.gameObject.name);
            Destroy(currentEdge.gameObject);
        }
    }
    /// <summary>
    /// SendMessage로 호출됨
    /// </summary>
    /// <param name="endTr"></param>
    public virtual void RemoveEdge(Transform endTr)
    {
        for(int i = edges.Count - 1; i >= 0; --i)
        {
            if (edges[i].endTarget.Equals(endTr))
            {
                edges.RemoveAt(i);
            }
        }
        IInputParam<T> inputParamT = endTr.GetComponent<IInputParam<T>>();
        for (int i=0; i < connectedInputModules.Count; ++i)
        {
            if (connectedInputModules[i].Equals(inputParamT))
            {
                connectedInputModules.RemoveAt(i);
                break;
            }
        }
        Debug.Log("Removed : " + endTr.gameObject.name);
    }

    public virtual void AfterInputCallBack() { }
    public void CreateDefaultEdges()
    {
        foreach (Transform targetTr in defaultEdge)
        {
            if(targetTr == null)
            {
                Debug.LogError($"{gameObject.name}의 Default Edge의 targetTr이 null입니다.");
                continue;
            }
            CreateEdge(targetTr);
        }
    }
    void OnDisable()
    {
        for (int i = 0; i < edges.Count; ++i)
        {
            if(!edges[i].bDestroyed)
                edges[i].gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        for (int i = 0; i < edges.Count; ++i)
        {
            if (!edges[i].bDestroyed)
                edges[i].gameObject.SetActive(true);
        }
    }
    public override void ExpandDisplay()
    {
        textMesh.text = input != null ? input.ToString() : "값없음";
    }
}

//변수Get 노드 부모
public abstract class GetValue : Node, IExpandableDisplay
{
    public bool isExpanded { get; set; }

    public override void Awake()
    {
        base.Awake();
        if (nodeName.StartsWith("Constant")) {
            string[] splitStr = { "Constant" };
            nodeName = nodeName.Split(splitStr,StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
    public virtual void Start()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("GetVariable CheckOutput()");
#endif
        CheckOutput();
    }
    public virtual void Update()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("GetVariable CheckOutput()");
#endif
        CheckOutput();
    }
    public void OnRenderObject()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("GetVariable OnRenderObject()");
#endif
        if (!isExpanded)
            nodeTitle.text = nodeName;
    }
}
public abstract class ConstantNode : GetValue
{

}
//SetVariable 노드 부모
/// <summary>
/// T는 대입될 데이터형
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SetVariable<T> : ActivatableNode, IInputParam<T>
{
    protected T value;
    [SerializeField]
    protected ActiveOutputModule active_output;
    
    public void Input(T input, int unused = 0)
    {
        if (input != null)
        {
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.NodeGotValue(gameObject.name, typeof(T));
            }
            value = input;
            CheckOutput();
        }
    }
    public void Input(T input)
    {
        Input(input, 0);
    }
    public override void Active()
    {
        base.Active();
        CheckOutput();
    }
    public virtual void Update()
    {
        value = default;
    }
}

//이벤트 노드 부모
public abstract class Event : Node
{
    public int isActive;
    public ActiveOutputModule active_output;
    private SpriteRenderer myRenderer;
    public void Active()
    {
        isActive = 2;
    }
    public override void Awake()
    {
        base.Awake();
        myRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Update()
    {
        isActive--;
    }
    public virtual void OnRenderObject()
    {
        UpdateDisplay();
    }
    public virtual void UpdateDisplay()
    {
        Color temp = myRenderer.color;
        temp.a = isActive >= 1 ? 1 : 0.5f;
        myRenderer.color = temp;
    }
    //Event노드는 CheckOutput이 필요하지 않습니다.
    public override void CheckOutput(){}
}
