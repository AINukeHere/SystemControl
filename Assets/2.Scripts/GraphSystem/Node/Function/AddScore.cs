using UnityEngine;

public class AddScore : ActivatableNode
{
    public BigInt value = null;
    public GameObject disappearText;

    public ActiveOutputModule active_output;

    public void Input(BigInt input, int unused = 0)
    {
        if (input != null)
        {
            value = input;
            CheckOutput();
        }
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && value != null)
        {
            ScoreGameManager.instance.AddScore(value);
            DisappearText text = Instantiate(disappearText, transform.position, Quaternion.identity).GetComponent<DisappearText>();
            text.text = value.ToString();
            active_output.Active();
            isActive--;
        }
    }
}
