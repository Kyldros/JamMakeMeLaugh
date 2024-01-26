using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public GameObject confirmationPanel;

    public ButtonSelection menuSelection;
    public ButtonSelection confirmationSelection;

    private ButtonSelection currentSelection;

    public void SelectionUp(InputAction.CallbackContext context)
    {
        if(currentSelection != null && context.canceled)
        {
            currentSelection.SelectButton(currentSelection.currentIndex - 1);
        }
    }

    public void SelectionDown(InputAction.CallbackContext context)
    {
        if (currentSelection != null && context.canceled)
        {
            currentSelection.SelectButton(currentSelection.currentIndex + 1);
        }
    }

    public void SetContinue()
    {
        if (startText != null)
        {
            startText.text = "Continue";
        }
    }

    public void SetStart()
    {
        if (startText != null)
        {
            startText.text = "Start";
        }
    }

    public void ExitMenu()
    {
        confirmationPanel.SetActive(true);
        currentSelection = confirmationSelection;
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

}
