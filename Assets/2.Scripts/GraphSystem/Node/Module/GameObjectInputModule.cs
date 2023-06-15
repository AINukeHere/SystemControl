using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameObjectParamEvent : UnityEvent<GameObject,int> { }
public class GameObjectInputModule : InputModule<GameObject>
{
    [SerializeField]
    public GameObjectParamEvent destination;
    [SerializeField]
    private int input_index;
    public override void Input(GameObject input)
    {
        base.Input(input);
        if (input != null)
        {
            destination.Invoke(input, input_index);
        }
    }
}
