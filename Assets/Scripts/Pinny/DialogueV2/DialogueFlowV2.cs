using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueFlowV2 : MonoBehaviour
{
    public FrasiDiPinny standardPinny;

    public Image pinnyimage;
    public TextMeshProUGUI pinnySpeech;
    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;
    public TextMeshProUGUI option3;

    public GameObject replyPanel;

    public List<AudioClip> selectionAudio;
    public List<AudioClip> pinnyAudio;

    private ButtonSelection currentSelection;

    private int dialogueIndex = 0;
    private FrasiDiPinny currentFrase;
    private bool isLastFrase = false;
    private bool called = false;

    private void Start()
    {
        currentSelection = replyPanel.GetComponent<ButtonSelection>();
        SetNewPrhase(standardPinny);
    }

    public void Selection(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y > 0.95 && !called)
        {
            SelectionUp();
            called = true;
        }

        if (context.ReadValue<Vector2>().y < -0.95 && !called)
        {
            SelectionDown();
            called = true;
        }

        if (context.ReadValue<Vector2>().y < 0.95 && context.ReadValue<Vector2>().y > -0.95)
            called = false;
    }

    public void SelectionUp()
    {
        if (currentSelection != null)
        {
            currentSelection.SelectButton(currentSelection.currentIndex - 1);
            GameManager.Instance.audioManager.PlayAudio(GetRandomAudio(selectionAudio));
        }
    }

    private AudioClip GetRandomAudio(List<AudioClip> audioClips)
    {
        if(audioClips.Count != 0)
            return audioClips[Random.Range(0, audioClips.Count)];
        else
            return null;
    }

    public void SelectionDown()
    {
        if (currentSelection != null)
        {
            currentSelection.SelectButton(currentSelection.currentIndex + 1);
            GameManager.Instance.audioManager.PlayAudio(GetRandomAudio(selectionAudio));
        }
    }

    public void StartPinnyWithPhrase(FrasiDiPinny startingPhrase)
    {
        SetNewPrhase(startingPhrase);
    }

    public void StartStandardPinny()
    {
        SetNewPrhase(standardPinny);
    }

    void SetNewPrhase(FrasiDiPinny frase)
    {
        DisableReply();
        if (frase != null)
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
        pinnySpeech.text = currentFrase.frasi[dialogueIndex].GetPrhase();
        if (currentFrase.frasi[dialogueIndex].sprite != null)
            pinnyimage.sprite = currentFrase.frasi[dialogueIndex].sprite;
        GameManager.Instance.audioManager.PlayAudio(GetRandomAudio(pinnyAudio));
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
            else
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                    EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }
        }

    }

    public void SelectOption1(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            ClickOption1();
    }

    public void ClickOption1()
    {
        SetNewPrhase(currentFrase.risposta1.PinnyReply);
    }

    public void SelectOption2(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            ClickOption2();
    }

    public void ClickOption2()
    {
        SetNewPrhase(currentFrase.risposta2.PinnyReply);
    }

    public void SelectOption3(InputAction.CallbackContext context)
    {
        if (isLastFrase && currentFrase.hasReply && context.canceled)
            ClickOption3();
    }

    public void ClickOption3()
    {
        SetNewPrhase(currentFrase.risposta3.PinnyReply);
    }

    private void UnselectAllButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
