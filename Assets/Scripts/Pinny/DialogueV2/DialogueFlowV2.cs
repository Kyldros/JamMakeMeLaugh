using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueFlowV2 : MonoBehaviour
{
    public FrasiDiPinny standardPinny;

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
        SetNewPrhase(standardPinny);
    }

    public void StartPinnyWithPhrase(FrasiDiPinny startingPhrase)
    {
        SetNewPrhase(startingPhrase);
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

       UnselectAllButton();
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
        GameManager.Instance.ClosePinny();
        currentFrase = standardPinny;
    }

    private void DisableReply()
    {
        replyPanel.SetActive(false);
        GameManager.Instance.SetCursorOff();
    }

    private void EnableReply()
    {
        replyPanel.SetActive(true);
        option1.text = currentFrase.risposta1.GetReply();
        option2.text = currentFrase.risposta2.GetReply();
        option3.text = currentFrase.risposta3.GetReply();
        GameManager.Instance.SetCursorOn();
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
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            ClickOption1();
    }

    public void ClickOption1()
    {
        SetNewPrhase(currentFrase.risposta1.pinnyReply);
    }

    public void SelectOption2(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            ClickOption2();
    }

    public void ClickOption2()
    {
        SetNewPrhase(currentFrase.risposta2.pinnyReply);
    }

    public void SelectOption3(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            ClickOption3();
    }

    public void ClickOption3()
    {
        SetNewPrhase(currentFrase.risposta3.pinnyReply);
    }

    private void UnselectAllButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
