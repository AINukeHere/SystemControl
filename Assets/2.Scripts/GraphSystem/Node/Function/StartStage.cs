using UnityEngine;

public class StartStage : ActivatableNode
{
    public int? value;
    public GameObject disappearText;

    public ActiveOutputModule active_output;

    public void Input(int? input, int unused = 0)
    {
        if (input != null)
        {
            value = input;
            CheckOutput();
        }
    }
    public void Update()
    {
        value = null;
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && value != null)
        {
#if UNITY_EDITOR
            Debug.Log("StageStart(" + value.ToString() + ")");
#endif
            if (AvoidGameManager.instance != null)
            {
                AvoidGameManager.instance.StartStage(value.Value);
                DisappearText text = Instantiate(disappearText, transform.position, Quaternion.identity).GetComponent<DisappearText>();
                text.text = value.ToString();
                value = null;
                active_output.Active();
                isActive--;
            }
        }
    }
}
