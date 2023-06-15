using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDownEvent : Event, IExpandableDisplay
{
    [SerializeField]
    private KeyCode key;


    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(key))
        {
            Active();
            active_output.Active();
        }
    }
    public bool isExpanded { get; set; }

    public override void UpdateDisplay()
    {
        base.UpdateDisplay();
        if (isExpanded)
            ExpandDisplay();
        else
            NormalDisplay();
    }
    public void ExpandDisplay()
    {
        if (nodeTitle != null)
            nodeTitle.text = $"{key.ToString()}";
    }

    public void NormalDisplay()
    {
        if (nodeTitle != null)
            nodeTitle.text = nodeName;
    }
}
