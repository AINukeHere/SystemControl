using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPlayEvent : Event
{
    public void Start()
    {
        Active();
        active_output.Active();
    }
}
