using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActivateType
{
    [SerializeField]
    public int isActive { get; private set; }
    public void Active()
    {
        isActive = 2;
    }
    public void Update()
    {
        isActive--;
    }
    public ActivateType() { isActive = 2; }
}
/// <summary>
/// 다른 모듈들은 Input이 호출되면 Input값을 업데이트하고 CheckOutput을 호출하여
/// 연결된 InputModule들에게 Input을 호출하지만 ActivateType 모듈은 Active로 신호를 전달받고
/// Input의 매개변수로 무조건 null이 전달되며 input 갱신은 반드시 Active메소드를 호출하고 
/// Update를 오버라이드하여 input.Update를 호출하여 isActive를 감소시킨다.
/// </summary>
public class ActiveOutputModule : OutputModule<ActivateType>
{
    protected override void Awake()
    {
        base.Awake();
        input = new ActivateType();
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("input = new ActivateType();");
#endif
    }
    public void Active()
    {
        /// 즉 OutputModule<T>의 base 메소드들을 호출하지 않는다.
        /// base.Input(new ActivateType());
        Input();
    }
    public override void Input(ActivateType _=null)
    {
        input.Active();
        CheckOutput();
    }
    public override void ResetInput()
    {
        input.Update();
    }
    public override void ExpandDisplay()
    {
        textMesh.text = input.isActive > 1 ? "실행중" : "신호없음";
    }
}