using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider_SwitchWall : MonoBehaviour
{
    [SerializeField]
    private int nSwitch;
    bool finalState = false;
    public bool state
    {
        get
        {
            return finalState;
        }
    }
    bool[] states;
    void Awake()
    {
        states = new bool[nSwitch];
    }
    void Update()
    {
        bool flag = true;
        foreach(bool b in states)
        {
            if (b == false)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            finalState = true;
            gameObject.SetActive(false);
        }
    }
    public void ReportSwitchState(int index, bool bSet)
    {
        states[index] = bSet;
    }
}
