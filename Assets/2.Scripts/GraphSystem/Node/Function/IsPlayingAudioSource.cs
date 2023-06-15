using UnityEngine;

public class IsPlayingAudioSource : MethodNode
{
    int isActive;
    private AudioSource value = null;
    public BoolOutputModule bool_output;

    public void Update()
    {
        value = null;
    }

    public void Input(AudioSource input, int unused = 0)
    {
        if (input != null)
        {
            value = input;
            CheckOutput();
        }
    }
    public override void CheckOutput()
    {
        if (value != null)
        {
            bool_output.Input(value.isPlaying);
            bool_output.CheckOutput();
        }
    }

    public override bool CheckRuningState()
    {
        return isActive > 0;
    }

}
