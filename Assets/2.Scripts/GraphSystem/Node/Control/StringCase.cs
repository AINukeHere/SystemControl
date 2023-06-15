using UnityEngine;
using System;
[System.Serializable]
public class StringCase : NotInputNotNodeActivatable
{
    public ActiveOutputModule active_output;
    public string case_value;
	//stringCase에 값이 들어올 때마다 SwitchOnString의 CheckOutput 호출
	private  SwitchOnString myHead;

    public void Input(string input, int unused = 0)
    {
        case_value = input;
        myHead.CheckOutput ();
    }
	public void SetSwitchHead(SwitchOnString head)
	{
		myHead = head;
	}
}