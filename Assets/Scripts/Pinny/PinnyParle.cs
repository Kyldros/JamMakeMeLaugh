using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PinnyParle : MonoBehaviour
{
    public Image pinnyimage;

    public TextMeshProUGUI pinnySpeech;
    public TextMeshProUGUI option1;
    public TextMeshProUGUI option2;
    public TextMeshProUGUI option3;

    public GameObject replyPanel;

    private int dialogueIndex = 0;
    public DialogueFlow dialogueFlow;
    private DialogueData currentDialogue;

    private void Start()
    {
        NextDialogue(dialogueFlow.dialogueList[0]);
    }

    public void SetText(string text, TextMeshProUGUI textbox )
    {
        textbox.text = text;
    }

    public void NextDialogue(DialogueData data)
    {
        if (data == null) 
            EndDialogue();
        else
            currentDialogue = data;

        DisableReply();
        SetText(data.pinnyText, pinnySpeech);
        if (data.haveReplyOptions)
        {
            EnableReply();
            SetText(data.option1.text, option1);
            SetText(data.option2.text, option2);
            SetText(data.option3.text, option3);
        }

    }

    public int OptionChosen(DilogueOptions options)
    {
        if(options == DilogueOptions.Option1)
            dialogueIndex = currentDialogue.option1.targetIndex;

        if (options == DilogueOptions.Option2)
            dialogueIndex = currentDialogue.option2.targetIndex;

        if (options == DilogueOptions.Option3)
            dialogueIndex = currentDialogue.option3.targetIndex;

        return dialogueIndex;
    }

    public void GoOn(InputAction.CallbackContext gigi)
    {
        if (gigi.canceled)
        {
            if (!currentDialogue.haveReplyOptions)
            {
                dialogueIndex++;
                if (dialogueIndex >= dialogueFlow.dialogueList.Count)
                {
                    EndDialogue();
                    return;
                }

                NextDialogue(dialogueFlow.dialogueList[dialogueIndex]);
            }
            Debug.Log("Go on");
        }
        
        
    }

    public void SelectOption1(InputAction.CallbackContext context)
    {
        if(currentDialogue.haveReplyOptions && context.canceled)
            NextDialogue(dialogueFlow.dialogueList[OptionChosen(DilogueOptions.Option1)]);
        
    }

    public void SelectOption2(InputAction.CallbackContext context)
    {
        if (currentDialogue.haveReplyOptions && context.canceled)
            NextDialogue(dialogueFlow.dialogueList[OptionChosen(DilogueOptions.Option2)]);
    }

    public void SelectOption3(InputAction.CallbackContext context)
    {
        if (currentDialogue.haveReplyOptions && context.canceled)
            NextDialogue(dialogueFlow.dialogueList[OptionChosen(DilogueOptions.Option3)]);
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
    }
}


public enum DilogueOptions
{
    Option1,
    Option2,
    Option3
}
