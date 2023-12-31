﻿using UnityEngine;

public class AvoiderReset : ActivatableNode
{
    private TextMesh myTextMesh;

    public GameObject disappearText;
    private Transform avoiderTr;

    public ActiveOutputModule active_output;

    public override void Active()
    {
        base.Active();
        CheckOutput();
    }
    public override void Awake()
    {
        base.Awake();
        myTextMesh = GetComponentInChildren<TextMesh>();
        avoiderTr = GameObject.FindGameObjectWithTag("Avoider").transform;
    }

    public override void Update()
    {
        base.Update();
        if (isActive >= 1)
        {
            //CheckOutput();
            isActive--;
        }
    }
    public override void CheckOutput()
    {
        if (isActive >= 2)
        {
            avoiderTr.position = AvoidGameManager.instance.GetStartPosition();
            active_output.Active();
            isActive--;
        }
    }

    public override string GetInfoString()
    {
        return "Avoider의 위치를 시작위치로 초기화시킵니다.";
    }
}
