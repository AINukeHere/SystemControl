using UnityEngine;

public class Raycast : ActivatableNode
{
    //output nodes
    [SerializeField] private ActiveOutputModule active_output;
    [SerializeField] private RaycastHitOutputModule raycastHit_output;
    //input values
    Ray? rayVal = null;
    float? maxDistanceVal = null;
    RaycastHit hitInfo;

    public void Input(Ray? input, int index = 0)
    {
        if (input != null)
        {
            rayVal = input;
            CheckOutput();
        }
    }
    public void Input(float? input, int index = 0)
    {
        if (input != null)
        {
            maxDistanceVal = input;
            CheckOutput();
        }
    }

    public void Update()
    {
        rayVal = null;
        maxDistanceVal = null;
    }
    public override void CheckOutput()
    {
		if (isActive >= 2 && rayVal.HasValue && maxDistanceVal.HasValue)
        {
            bool res = Physics.Raycast(rayVal.Value, out hitInfo, maxDistanceVal.Value);
            active_output.Active();
            isActive--;
        }
    }
}
