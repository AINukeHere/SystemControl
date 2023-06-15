using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum ModuleValueType
{
    Active,
    @int,
    @float,
    @string,
    Vector2,
    Vector3,
    @bool,
    AudioClip,
    AudioSource,
    Component,
    Ray,
    RaycastHit,
    Camera,

    MAX
}
public class NodeManager : MonoBehaviour
{
    public static NodeManager instance { get; private set; }
    private Color[] typeColorInfo;
    private Dictionary<Type, string> dict_nodeType_explainText;
    private Dictionary<Type, string> dict_nodeType_nodeName;
    private Dictionary<Type, string> dict_nodeType_inputNormalDisplay;
    private Dictionary<Type, string> dict_nodeType_outputNormalDisplay;
    private Dictionary<Type, string> dict_valueType_valueTypeString;
    void Awake()
    {
        if (instance != null) { Destroy(gameObject);
            return; }

        instance = this;
        isExpandDisplay = false;

        if (toogleEyeImg != null)
            toogleEyeImg.sprite = eye_closed_img;
        InitializeNodeInfo();
    }
    public static ArrayList GetSubClasses(string asmPath, Type baseClassType)
    {
        Assembly asm = Assembly.Load(asmPath);
        ArrayList subClasses = new ArrayList();
        if (baseClassType != null)
        {
            foreach (Type type in asm.GetTypes())
            {
                if (type.IsSubclassOf(baseClassType))
                {
                    subClasses.Add(type);
                }
            }
        }
        else
        {
            throw (new Exception(baseClassType.FullName +
                            " does not exist in assembly " + asmPath));
        }

        return (subClasses);
    }
    public void InitializeNodeInfo()
    {
        //모듈 타입별 색상 관리
        typeColorInfo = new Color[(int)ModuleValueType.MAX];
        typeColorInfo[(int)ModuleValueType.Active] = Color.white;
        typeColorInfo[(int)ModuleValueType.@int] = new Color32(30, 226, 174, 255);
        typeColorInfo[(int)ModuleValueType.@float] = Color.green;
        typeColorInfo[(int)ModuleValueType.@string] = Color.magenta;
        typeColorInfo[(int)ModuleValueType.Vector2] = new Color32(231, 182, 35, 255);
        typeColorInfo[(int)ModuleValueType.Vector3] = new Color32(231, 182, 35, 255);
        typeColorInfo[(int)ModuleValueType.@bool] = Color.red;
        typeColorInfo[(int)ModuleValueType.AudioClip] = new Color32(128, 64, 0, 255);
        typeColorInfo[(int)ModuleValueType.AudioSource] = new Color32(0, 168, 242, 255);
        typeColorInfo[(int)ModuleValueType.Component] = new Color32(0, 168, 242, 255);
        typeColorInfo[(int)ModuleValueType.Ray] = new Color32(0, 168, 242, 255);
        typeColorInfo[(int)ModuleValueType.RaycastHit] = new Color32(0, 168, 242, 255);
        typeColorInfo[(int)ModuleValueType.Camera] = new Color32(0, 168, 242, 255);

        dict_nodeType_explainText = new Dictionary<Type, string>();
        dict_nodeType_nodeName = new Dictionary<Type, string>();
        dict_nodeType_inputNormalDisplay = new Dictionary<Type, string>();
        dict_nodeType_outputNormalDisplay = new Dictionary<Type, string>();
        dict_valueType_valueTypeString = new Dictionary<Type, string>();

        dict_valueType_valueTypeString[typeof(ActivateType)] = "실행";
        dict_valueType_valueTypeString[typeof(AudioClip)] = "AudioClip";
        dict_valueType_valueTypeString[typeof(bool?)] = "bool";
        dict_valueType_valueTypeString[typeof(Camera)] = "Camera";
        dict_valueType_valueTypeString[typeof(Component)] = "Component";
        dict_valueType_valueTypeString[typeof(float?)] = "int";
        dict_valueType_valueTypeString[typeof(GameObject)] = "int";
        dict_valueType_valueTypeString[typeof(int?)] = "int";
        dict_valueType_valueTypeString[typeof(RaycastHit?)] = "RaycastHit";
        dict_valueType_valueTypeString[typeof(Ray?)] = "Ray";
        dict_valueType_valueTypeString[typeof(string)] = "string";
        dict_valueType_valueTypeString[typeof(Vector2?)] = "Vector2";
        dict_valueType_valueTypeString[typeof(Vector3?)] = "Vector3";
        // 노드의 이름을 노드타입명으로 기본설정
        var subClasses = GetSubClasses("Assembly-CSharp", typeof(Node));
        foreach (Type subclass in subClasses)
        {
            dict_nodeType_nodeName[subclass] = subclass.ToString();
        }

        //커스터마이징 부분
        #region Control
        /*Branch*/
        dict_nodeType_explainText.Add(typeof(Branch), "입력된 참또는 거짓에 따라 참일경우 실행신호를 오른쪽상단모듈로 보내고 거짓일 경우 오른쪽 하단모듈로 내보냅니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(Branch), "Active\n\nBool");
        dict_nodeType_explainText.Add(typeof(SwitchOnString), "입력된 문자열을 보고 같은 문자열이 있으면 해당 문자열의 바로 우측으로 실행신호를 보내고 하나도 매칭이 안된다면 맨 상단으로 내보냅니다.");
        #endregion
        #region Event
        dict_nodeType_explainText.Add(typeof(AvoiderCollision), "Avoider와 충돌하고있는 것들의 이름을 내보냅니다. 여러개라면 여러번 실행될 수 있습니다.");
        dict_nodeType_outputNormalDisplay.Add(typeof(AvoiderCollision), "Active\n\nstring");
        dict_nodeType_explainText.Add(typeof(BeginPlayEvent), "맨 처음 한번만 실행신호를 내보냅니다.");
        dict_nodeType_outputNormalDisplay.Add(typeof(BeginPlayEvent), "Active");
        dict_nodeType_explainText.Add(typeof(KeyDownEvent), "특정 키가 입력되면 실행신호를 내보냅니다.");
        dict_nodeType_outputNormalDisplay.Add(typeof(KeyDownEvent), "Active");
        dict_nodeType_explainText.Add(typeof(LateTickEvent), "매 프레임마다 실행신호를 내보냅니다. 하지만 TickEvent보다는 늦게 내보냅니다.");
        dict_nodeType_outputNormalDisplay.Add(typeof(LateTickEvent), "Active");
        dict_nodeType_explainText.Add(typeof(TickEvent), "매 프레임마다 실행신호를 내보냅니다.");
        dict_nodeType_outputNormalDisplay.Add(typeof(TickEvent), "Active");
        dict_nodeType_explainText.Add(typeof(TouchEvent), "사용자가 클릭을 했을 때 실행신호를 내보냅니다.");
        dict_nodeType_outputNormalDisplay.Add(typeof(TouchEvent), "Active");
        #endregion
        #region Function
        dict_nodeType_explainText.Add(typeof(AddScore), "현재 Score에 이 노드로 입력된 값만큼 더합니다."); //unused
        dict_nodeType_explainText.Add(typeof(AvoiderReset), "Avoider의 위치, 크기, 속도를 초기화시킵니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(AvoiderReset), "Active");
        dict_nodeType_outputNormalDisplay.Add(typeof(AvoiderReset), "Active");
        dict_nodeType_explainText.Add(typeof(GetComponent), "입력된 게임오브젝트의 선택된 컴포넌트를 내보냅니다."); //unprepared
        dict_nodeType_explainText.Add(typeof(IsPlayingAudioSource), "입력된 음원이 현재 실행중인지를 내보냅니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(IsPlayingAudioSource), "AudioClip");
        dict_nodeType_outputNormalDisplay.Add(typeof(IsPlayingAudioSource), "bool");
        dict_nodeType_explainText.Add(typeof(LoadScene), "입력된 씬이름의 씬을 로드합니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(LoadScene), "Active\n\nstring");
        dict_nodeType_outputNormalDisplay.Add(typeof(LoadScene), "Active");
        dict_nodeType_explainText.Add(typeof(PlayAudioClip), "입력된 음원을 실행합니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(PlayAudioClip), "Active\n\nAudioClip");
        dict_nodeType_outputNormalDisplay.Add(typeof(PlayAudioClip), "Active");
        dict_nodeType_explainText.Add(typeof(Raycast), "입력된 Ray를 쏜 결과값을 내보냅니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(Raycast), "Active\n\nRay");
        dict_nodeType_outputNormalDisplay.Add(typeof(Raycast), "Active\n\nRaycastHit");
        dict_nodeType_explainText.Add(typeof(ScreenPointToRay), "입력된 카메라의 뷰에서 위치한 입력된 벡터위치로의 Ray를 내보냅니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(ScreenPointToRay), "Active\n\nCamera\n\nVector3");
        dict_nodeType_outputNormalDisplay.Add(typeof(ScreenPointToRay), "Active\n\n\n\nRay");
        dict_nodeType_explainText.Add(typeof(SetActiveWall), "입력된 두 점을 잇는 벽을 입력된 참또는 거짓에 따라 생성하거나 제거합니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(SetActiveWall), "Active\n\nint\n\nint\n\nbool");
        dict_nodeType_outputNormalDisplay.Add(typeof(SetActiveWall), "Active");
        dict_nodeType_explainText.Add(typeof(StartStage), "입력된 값의 스테이지를 시작합니다.");
        dict_nodeType_inputNormalDisplay.Add(typeof(StartStage), "Active\n\nint");
        dict_nodeType_outputNormalDisplay.Add(typeof(StartStage), "Active");
        dict_nodeType_explainText.Add(typeof(SubtractScore), "입력된 값만큼 점수를 낮춥니다."); //unused
        #endregion
        #region Operator
        dict_nodeType_explainText.Add(typeof(Absolute_Float), "입력된 실수값의 절대값을 내보냅니다.");
        dict_nodeType_nodeName[typeof(Absolute_Float)] = "│Float│";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Absolute_Float), "float");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Absolute_Float), "float");
        dict_nodeType_explainText.Add(typeof(Add_Float), "입력된 두 실수값을 더한 결과를 내보냅니다.");
        dict_nodeType_nodeName[typeof(Add_Float)] = "＋";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Add_Float), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Add_Float), "float");
        dict_nodeType_explainText.Add(typeof(Add_Int), "입력된 두 정수값을 더한 결과를 내보냅니다.");
        dict_nodeType_nodeName[typeof(Add_Int)] = "＋";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Add_Int), "int\n\nint");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Add_Int), "int");
        dict_nodeType_explainText.Add(typeof(BreakVector2), "입력된 벡터2를 스칼라값(실수) 둘로 쪼개서 각각 내보냅니다.");
        //dict_nodeType_inputNormalDisplay.Add(typeof(BreakVector2), "Vector2");
        //dict_nodeType_outputNormalDisplay.Add(typeof(BreakVector2), "float\n\nfloat");
        dict_nodeType_explainText.Add(typeof(BreakVector3), "입력된 벡터3를 스칼라값(실수) 셋으로 쪼개서 각각 내보냅니다.");
        //dict_nodeType_inputNormalDisplay.Add(typeof(BreakVector3), "Vector3");
        //dict_nodeType_outputNormalDisplay.Add(typeof(BreakVector3), "float\n\nfloat\n\nfloat");
        dict_nodeType_explainText.Add(typeof(Equal_Float), "입력된 두 실수가 같은지 판별하여 참 또는 거짓을 내보냅니다. ※주의! 컴퓨터가 실수를 저장하는 방식의 특성상 이 노드는 정상작동하기가 쉽지 않습니다.");
        dict_nodeType_nodeName[typeof(Equal_Float)] = "==";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Equal_Float), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Equal_Float), "bool");
        dict_nodeType_explainText.Add(typeof(Equal_String), "입력된 두 문자열이 같은지 판별하여 참 또는 거짓을 내보냅니다.");
        dict_nodeType_nodeName[typeof(Equal_String)] = "==";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Equal_String), "string\n\nstring");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Equal_String), "bool");
        dict_nodeType_explainText.Add(typeof(Greater_Float), "입력된 첫번째 실수값이 두번째 실수값보다 큰지 비교하여 참 또는 거짓을 내보냅니다.");
        dict_nodeType_nodeName[typeof(Greater_Float)] = "＞";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Greater_Float), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Greater_Float), "bool");
        dict_nodeType_explainText.Add(typeof(Greater_Int), "입력된 첫번째 정수값이 두번째 실수값보다 큰지 비교하여 참 또는 거짓을 내보냅니다.");
        dict_nodeType_nodeName[typeof(Greater_Int)] = "＞";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Greater_Int), "int\n\nint");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Greater_Int), "bool");
        dict_nodeType_explainText.Add(typeof(Int2Float), "입력된 정수값을 실수값으로 변환하여 내보냅니다.");
        dict_nodeType_nodeName[typeof(Int2Float)] = "(float)";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Int2Float), "int");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Int2Float), "float");
        dict_nodeType_explainText.Add(typeof(Less_Float), "입력된 첫번째 실수값이 두번째 실수값보다 작은지 비교하여 참 또는 거짓을 내보냅니다.");
        dict_nodeType_nodeName[typeof(Less_Float)] = "＜";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Less_Float), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Less_Float), "bool");
        dict_nodeType_explainText.Add(typeof(Less_Int), "입력된 첫번째 정수값이 두번째 실수값보다 작은지 비교하여 참 또는 거짓을 내보냅니다.");
        dict_nodeType_nodeName[typeof(Less_Int)] = "＜";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Less_Int), "int\n\nint");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Less_Int), "bool");
        dict_nodeType_explainText.Add(typeof(MakeVector2), "입력된 두 실수값으로 벡터2를 만들어 내보냅니다.");
        //dict_nodeType_inputNormalDisplay.Add(typeof(MakeVector2), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(MakeVector2), "Vector2");
        dict_nodeType_explainText.Add(typeof(MakeVector3), "입력된 세 실수값으로 벡터3를 만들어 내보냅니다.");
        //dict_nodeType_inputNormalDisplay.Add(typeof(MakeVector3), "float\n\nfloat\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(MakeVector3), "Vector3");
        dict_nodeType_explainText.Add(typeof(Multiply_Float), "입력된 두 실수를 곱한 결과를 내보냅니다.");
        dict_nodeType_nodeName[typeof(Multiply_Float)] = "×";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Multiply_Float), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Multiply_Float), "float");
        dict_nodeType_explainText.Add(typeof(Subtract_Float), "입력된 첫번째 실수에서 두번째 실수를 뺀 결과를 내보냅니다.");
        dict_nodeType_nodeName[typeof(Subtract_Float)] = "－";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Subtract_Float), "float\n\nfloat");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Subtract_Float), "float");
        dict_nodeType_explainText.Add(typeof(Subtract_Int), "입력된 첫번째 정수에서 두번째 정수를 뺀 결과를 내보냅니다.");
        dict_nodeType_nodeName[typeof(Subtract_Int)] = "－";
        //dict_nodeType_inputNormalDisplay.Add(typeof(Subtract_Int), "int\n\nint");
        //dict_nodeType_outputNormalDisplay.Add(typeof(Subtract_Int), "int");
        #endregion
        #region Variable
        dict_nodeType_explainText.Add(typeof(ConstantAudioClip), "음원입니다. 상수이므로 절대 변경되지 않습니다.");
        dict_nodeType_explainText.Add(typeof(ConstantBool), "참 또는 거짓을 나타내는 부울값입니다. 상수이므로 절대 변경되지 않습니다.");
        dict_nodeType_explainText.Add(typeof(ConstantFloat), "실수값입니다. 상수이므로 절대 변경되지 않습니다.");
        dict_nodeType_explainText.Add(typeof(ConstantInt), "정수값입니다. 상수이므로 절대 변경되지 않습니다.");
        dict_nodeType_explainText.Add(typeof(ConstantString), "문자열입니다. 상수이므로 절대 변경되지 않습니다.");
        dict_nodeType_explainText.Add(typeof(ConstantVector2), "Vector2입니다. 상수이므로 절대 변경되지 않습니다.");
        dict_nodeType_explainText.Add(typeof(GetAvoiderScale), "Avoider의 크기를 벡터2로 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetAvoiderVelocity), "Avoider의 현재 속도(방향이 있는 값)를 벡터2로 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetAvoiderX), "Avoider의 x좌표를 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetAvoiderY), "Avoider의 y좌표를 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetCurrentStageNum), "현재 스테이지 값을 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetDeltaTime), "이전프레임에서 현재프레임사이에 흐른 시간을 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetMainCamera), "메인(보통 게임을 비추고있는) 카메라 컴포넌트를 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetMainCameraPos), "메인(보통 게임을 비추고있는) 카메라의 좌표를 벡터3로 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetMousePosition), "스크린상의 마우스좌표를 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetHorizontalInput), "플레이어의 좌/우 키에 대한 입력값을 내보냅니다. (-1또는 0 또는 1)");
        dict_nodeType_explainText.Add(typeof(GetNodeCameraX), "노드를 비추고있는 카메라의 X좌표를 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetNodeCameraY), "노드를 비추고있는 카메라의 Y좌표를 내보냅니다.");
        dict_nodeType_explainText.Add(typeof(GetVerticalInput), "플레이어의 상/하 키에 대한 입력값을 내보냅니다. (-1또는 0 또는 1)");
        dict_nodeType_explainText.Add(typeof(SetAvoiderGravityScale), "입력된 실수로 Avoider의 중력크기를 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetAvoiderScale), "입력된 벡터2로 Avoider의 크기값을 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetAvoiderVelocity), "입력된 벡터2로 Avoider의 속도(방향이 있는 값)를 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetAvoiderX), "입력된 실수값으로 Avoider의 x값 좌표를 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetAvoiderY), "입력된 실수값으로 Avoider의 y값 좌표를 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetMainCameraPos), "입력된 벡터3로 메인(보통 게임을 비추고있는) 카메라의 위치를 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetNodeCameraX), "입력된 실수값으로 노드를 비추고 있는 카메라의 x좌표를 새로 설정합니다.");
        dict_nodeType_explainText.Add(typeof(SetNodeCameraY), "입력된 실수값으로 노드를 비추고 있는 카메라의 y좌표를 새로 설정합니다.");
        #endregion
    }

    public void Btn_ResetNodes()
    {
        //전체 엣지 삭제
        EdgeManager.instance.RemoveAllEdge();

        //노드초기화
        Node[] all_nodes = FindObjectsOfType<Node>();
        foreach (Node node in all_nodes)
            node.ResetNode();
        //모든 OutputModule에서 CreateDefaultEdges 호출
        List<Component> modules = new List<Component>();
        modules.AddRange(FindObjectsOfType<ActiveOutputModule>());
        modules.AddRange(FindObjectsOfType<AudioClipOutputModule>());
        modules.AddRange(FindObjectsOfType<BoolOutputModule>());
        modules.AddRange(FindObjectsOfType<FloatOutputModule>());
        modules.AddRange(FindObjectsOfType<IntOutputModule>());
        modules.AddRange(FindObjectsOfType<StringOutputModule>());
        modules.AddRange(FindObjectsOfType<Vector2OutputModule>());
        modules.AddRange(FindObjectsOfType<Vector3OutputModule>());
        foreach (var module in modules)
            module.SendMessage("CreateDefaultEdges");

        //EdgeManager.instance.UpdateAll();

        if (TutorialManager.instance != null)
			TutorialManager.instance.ConditionSatisfied (TutorialManager.ConditionType.RESET_NODE);
    }
	public Image toogleEyeImg;
    public bool isExpandDisplay { get; private set; }
    public Sprite eye_closed_img;
	public Sprite eye_opened_img;
	public void Btn_ToogleEye()
	{
		isExpandDisplay = !isExpandDisplay;

		//이미지 교체
        if (toogleEyeImg != null)
		    toogleEyeImg.sprite = isExpandDisplay ? eye_opened_img : eye_closed_img;

		//확장형 디스플레이에서 확장디스플레로 변경
		UpdateIsExpandedInAllNodes();
	}
    void UpdateIsExpandedInAllNodes()
    {
        Node[] all_nodes = FindObjectsOfType<Node>();
        ModuleColorizable[] all_modules = FindObjectsOfType<ModuleColorizable>();
        int max_count = 0;
        max_count = Mathf.Max(all_nodes.Length, all_modules.Length);

        Node node = null;
        ModuleColorizable module = null;
        for (int i = 0; i < max_count; ++i)
        {
            node = null;
            module = null;
            if (i < all_nodes.Length)
                node = all_nodes[i];
            if (i < all_modules.Length)
                module = all_modules[i];

            if (node != null)
            {
                IExpandableDisplay display = node.GetComponent<IExpandableDisplay>();
                if (display != null)
                {
                    display.isExpanded = isExpandDisplay;
                }
            }
            if (module != null)
            {
                module.isExpanded = isExpandDisplay;
            }
        }
    }
    private void ShutDownAllNode()
    {
        Node[] all_nodes = FindObjectsOfType<Node>();
        Debug.Log(all_nodes.Length + "개의 노드 비활성화");
        foreach (Node node in all_nodes)
            node.gameObject.SetActive(false);
    }

    public Color getModuleTypeColor(ModuleValueType valueType)
    {
        return typeColorInfo[(int)valueType];
    }
    public string getExplainText(Type nodeClassType)
    {
        return dict_nodeType_explainText[nodeClassType];
    }
    public string getNodeName(Type nodeClassType)
    {
        if (dict_nodeType_nodeName.ContainsKey(nodeClassType))
            return dict_nodeType_nodeName[nodeClassType];
        Debug.LogWarning($"getNodeName returned just class name : {nodeClassType.ToString()}");
        return nodeClassType.ToString();
    }
    public string getInputNormalDisplay(Type nodeClassType)
    {
        //return "";
        if (dict_nodeType_inputNormalDisplay.ContainsKey(nodeClassType))
            return dict_nodeType_inputNormalDisplay[nodeClassType];
        Debug.LogWarning($"getInputNormalDisplay returned just empty string from node : {nodeClassType.ToString()}");
        return "";
    }
    public string getOutputNormalDisplay(Type nodeClassType)
    {
        //return "";
        if (dict_nodeType_outputNormalDisplay.ContainsKey(nodeClassType))
            return dict_nodeType_outputNormalDisplay[nodeClassType];
        Debug.LogWarning($"getOutputNormalDisplay returned just empty string from node : {nodeClassType.ToString()}");
        return "";
    }
    public string getValueTypeName(Type valueType)
    {
        if (dict_valueType_valueTypeString.ContainsKey(valueType))
            return dict_valueType_valueTypeString[valueType];
        Debug.LogError($"could not found value type name : {valueType.ToString()}");
        return "UNKNOWN";
    }
    public string getModuleType(Type moduleType)
    {
        string[] splitStrings = { "InputModule", "OutputModule" };
        string result = moduleType.ToString();
        result = result.Split(splitStrings, StringSplitOptions.RemoveEmptyEntries)[0];
        return result;
    }
}