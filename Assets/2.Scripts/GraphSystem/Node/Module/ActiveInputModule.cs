using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ActiveParamEvent : UnityEvent<ActivateType, int> { }
public class ActiveInputModule : InputModule<ActivateType>
{
    [SerializeField]
    public ActiveParamEvent destination;
    [SerializeField]
    private int input_index;
    protected override void Awake()
    {
        base.Awake();
        input_for_visualize = new ActivateType();
    }
    public override void Input(ActivateType input)
    {
        input_for_visualize.Active();
        destination.Invoke(null, input_index);
    }
    public override void ResetInputForVisualize()
    {
        input_for_visualize.Update();
    }

    public override void NormalDisplay()
    {
        textMesh.text = "Active";
    }
    public override void ExpandDisplay()
    {
        textMesh.text = input_for_visualize.isActive > 1 ? "실행중" : "신호없음";
    }
}