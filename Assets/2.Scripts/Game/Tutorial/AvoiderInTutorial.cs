using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoiderInTutorial : MonoBehaviour {

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("NextStage"))
            TutorialManager.instance.StageClear();
    }
}
