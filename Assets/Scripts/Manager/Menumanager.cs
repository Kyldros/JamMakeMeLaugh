using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image startImageReference;
    public Sprite startSprite;
    public Sprite continueSprite;
    public Sprite restartSprite;

    public GameObject confirmationPanel;

    public ButtonSelection menuSelection;
    public ButtonSelection confirmationSelection;
    public List<AudioClip> selectionMenu;
    private ButtonSelection currentSelection;

    public Image titleImage;
    public Sprite loseSprite;
    public Sprite winSprite;

    private void Start()
    {
        currentSelection = menuSelection;
    }

    public void ClickButton(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if(EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void Selection(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y > 0)
            SelectionUp();
        else if(context.ReadValue<Vector2>().y < 0)
            SelectionDown();

        GameManager.Instance.audioManager.PlayAudio(GetRandomAudio(selectionMenu));
    }

    public void SelectionUp()
    {
        if(currentSelection != null)
        {
            currentSelection.SelectButton(currentSelection.currentIndex - 1);
        }
    }

    public void SelectionDown()
    {
        if (currentSelection != null)
        {
            currentSelection.SelectButton(currentSelection.currentIndex + 1);
        }
    }

    public void SetContinue()
    {
        if (startImageReference != null)
        {
            startImageReference.sprite = continueSprite;
        }
    }

    public void SetStart()
    {
        if (startImageReference != null)
        {
            startImageReference.sprite = startSprite;
        }
    }

    public void SetRestart()
    {
        if (startImageReference != null)
        {
            startImageReference.sprite = restartSprite;
        }
    }

    public void SetWinImage()
    {
        titleImage.sprite = winSprite;
    }
    public void SetLoseImage()
    {
        titleImage.sprite = loseSprite;
    }

    public void ExitMenu()
    {
        confirmationPanel.SetActive(true);
        currentSelection = confirmationSelection;
        SelectionUp();
    }
    public void ExitExitMenu()
    {
        confirmationPanel.SetActive(false);
        currentSelection = menuSelection;
    }

    public void ExitConfirmation() 
    { 
        Application.Quit();
    }

    private AudioClip GetRandomAudio(List<AudioClip> audioClips)
    {
        if (audioClips.Count != 0)
            return audioClips[Random.Range(0, audioClips.Count)];
        else
            return null;
    }

    public void WinGame()
    {
        SetRestart();
        SetWinImage();
    }

    public void LoseGame()
    {
        SetRestart();
        SetLoseImage();
    }

}
