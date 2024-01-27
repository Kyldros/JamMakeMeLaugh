using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinnySensateDialogueTrigger : MonoBehaviour
{
    public FrasiDiPinny sensateAnswer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            if(sensateAnswer != null)
                GameManager.Instance.pinny.GetComponent<DialogueFlowV2>().standardPinny = sensateAnswer;
        }
    }
}
