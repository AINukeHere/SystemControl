using UnityEngine;

public class SubtractScore : ActivatableNode
{
    public BigInt value = null;
    public GameObject disappearText;

    public ActiveOutputModule active_output;


    public void Input(BigInt input, int unused = 0)
    {
        if (input != null)
        {
            value = input.Clone();
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
            ScoreGameManager.instance.SubtractScore(value);
            DisappearText text = Instantiate(disappearText, transform.position, Quaternion.identity).GetComponent<DisappearText>();
            text.text = (-value).ToString();
            value = null;
            active_output.Active();
            isActive--;
        }
    }
}
