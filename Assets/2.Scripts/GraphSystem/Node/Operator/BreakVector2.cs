using UnityEngine;

public class BreakVector2 : Operator<Vector2?,float?>
{
    public override void CheckOutput()
    {
#if UNITY_EDITOR
        if (gameObject.name.EndsWith("(Test)"))
            Debug.Log("BreakVector2 CheckOutput()");
#endif
        if (input[0] != null)
        {
            output[0] = input[0].Value.x;
            output[1] = input[0].Value.y;
            outputModules[0].Input(output[0]);
            outputModules[1].Input(output[1]);
        }
    }
}
