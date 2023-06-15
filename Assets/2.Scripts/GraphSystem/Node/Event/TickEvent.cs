using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickEvent : Event
{
    public override void Update()
    {
        base.Update();
        Active();
        active_output.Active();
    }
}
