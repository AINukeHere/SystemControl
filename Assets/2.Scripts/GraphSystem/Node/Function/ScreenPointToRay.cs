using UnityEngine;

public class ScreenPointToRay : ActivatableNode
{
    [SerializeField] private ActiveOutputModule active_output;
    [SerializeField] private RayOutputModule ray_output;
    //values
    Camera cameraVal = null;
    Vector3? vec3Val = null;
    Ray? rayVal = null;

    public void Input(Vector3? input, int index = 0)
    {
        if (input != null)
        {
            vec3Val = input;
            CheckOutput();
        }
    }
    public void Input(Camera input, int index = 0)
    {
        if (input != null)
        {
            cameraVal = input;
            CheckOutput();
        }
    }

    public void Update()
    {
        vec3Val = null;
        cameraVal = null;
        rayVal = null;
    }
    public override void CheckOutput()
    {
		if (isActive >= 2 && vec3Val != null && cameraVal != null)
        {
            rayVal = cameraVal.ScreenPointToRay(vec3Val.Value);
            ray_output.Input(rayVal);
            active_output.Active();
            isActive--;
        }
    }
}
