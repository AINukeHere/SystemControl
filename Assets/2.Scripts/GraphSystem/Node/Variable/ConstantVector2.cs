using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantVector2 : ConstantNode
{
	[SerializeField]
	private Vector2 value = Vector2.zero;
    [SerializeField]
    private Vector2OutputModule vector2OutputModule;
	public override void CheckOutput()
	{
        vector2OutputModule.Input(value);
    }
}