using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinnyDialogueTrigger : MonoBehaviour
{
    public FrasiDiPinny dialogueToStart;
    public bool oneTimeOnly = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            GameManager.Instance.pinny.GetComponent<DialogueFlowV2>().StartPinnyWithPhrase(dialogueToStart);
            GameManager.Instance.OpenPinny();
            if (oneTimeOnly)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
