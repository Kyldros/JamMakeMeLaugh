using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//[CreateAssetMenu(fileName = "Dialogo", menuName = "Dialoghi/Frasi/Dialogo")]
public class DialogueFlowV2 : MonoBehaviour
{
    public FrasiDiPinny fraseIniziale;
    //public List<FrasiDiPinny> frasi = new List<FrasiDiPinny>();
    //public List<Risposte> risposte = new List<Risposte>();
    public Image pinnyimage;
    public TextMeshProUGUI pinnySpeech;
    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;
    public TextMeshProUGUI option3;

    public GameObject replyPanel;

    private int dialogueIndex = 0;
    private FrasiDiPinny currentFrase;
    private bool isLastFrase = false;

    private void Start()
    {
        SetNewPrhase(fraseIniziale);
    }


    void SetNewPrhase(FrasiDiPinny frase)
    {
        DisableReply();
        if(frase != null)
        {
            currentFrase = frase;
            dialogueIndex = 0;
            SetTextAndImage();
            CheckLastPhrase();
            CheckReply();
        }
        else 
            Debug.Log("Frase non trovata");

    }

    private void SetTextAndImage()
    {
        pinnySpeech.text = currentFrase.frasi[dialogueIndex].frase;
        if(currentFrase.frasi[dialogueIndex].sprite != null)
            pinnyimage.sprite = currentFrase.frasi[dialogueIndex].sprite;
    }

    public void NextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < currentFrase.frasi.Count)
        {
            SetTextAndImage();
            CheckLastPhrase();
        }

        CheckReply();
    }

    private void CheckReply()
    {
        if (currentFrase.hasReply && isLastFrase)
        {
            EnableReply();
        }
    }

    private void CheckLastPhrase()
    {
        if (dialogueIndex + 1 < currentFrase.frasi.Count)
            isLastFrase = false;
        else
            isLastFrase = true;
    }


    private void EndDialogue()
    {
        Debug.Log("End dialogue");
    }

    private void DisableReply()
    {
        replyPanel.SetActive(false);
    }

    private void EnableReply()
    {
        replyPanel.SetActive(true);
        option1.text = currentFrase.risposta1.GetReply();
        option2.text = currentFrase.risposta2.GetReply();
        option3.text = currentFrase.risposta3.GetReply();
    }

    public void GoOn(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (isLastFrase && !currentFrase.hasReply)
                EndDialogue();
            else if (!isLastFrase)
                NextDialogue();
        }
        
    }

    public void SelectOption1(InputAction.CallbackContext context)
    {
        if(isLastFrase && currentFrase.hasReply && context.canceled)
            SetNewPrhase(currentFrase.risposta1.pinnyReply);
    }

    public void SelectOption2(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            SetNewPrhase(currentFrase.risposta2.pinnyReply);
    }

    public void SelectOption3(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            SetNewPrhase(currentFrase.risposta3.pinnyReply);
    }

}
