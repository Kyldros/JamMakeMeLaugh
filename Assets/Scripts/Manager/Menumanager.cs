using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI startText;
    public GameObject confirmationPanel;

    public ButtonSelection menuSelection;
    public ButtonSelection confirmationSelection;
    private bool gameStarted = false;
    private ButtonSelection currentSelection;

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

}
