using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : ActivatableNode
{
    public ActiveOutputModule active_output;
    //values
    string stringVal = null;

    public void Input(string input, int index = 0)
    {
        if (input != null)
        {
            stringVal = input;
            CheckOutput();
        }
    }

    public void Update()
    {
        stringVal = null;
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && stringVal != null)
        {
            SceneManager.LoadScene(stringVal);
            active_output.Active();
            isActive--;
        }
    }
}
