using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateTickEvent : Event
{
    public void LateUpdate()
    {
        Active();
        active_output.Active();
    }
}
