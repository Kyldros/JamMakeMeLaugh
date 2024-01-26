using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject pinny;
    public GameObject menu;

    InputScheme playScheme = InputScheme.Menu;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    public void OpenPinny()
    {
        if (pinny != null && !pinny.gameObject.activeInHierarchy)
        {
            pinny.gameObject.SetActive(true);
            ChangeInputScheme(InputScheme.PinnyDialogue);
        }
    }

    public void ClosePinny()
    {
        if (pinny != null && pinny.gameObject.activeInHierarchy)
        {
            pinny.gameObject.SetActive(false);
            ChangeInputScheme(InputScheme.Player);
        }
    }

    public void ChangeInputScheme(InputScheme scheme)
    {
        playerInput.SwitchCurrentActionMap(scheme.ToString());
        if(scheme != InputScheme.Menu)
            playScheme = scheme;
    }
    
    public void OpenCloseMenu()
    {
        if(menu != null)
        {
            if (menu.gameObject.activeInHierarchy)
            {
                menu.gameObject.SetActive(false);
                ChangeInputScheme(playScheme);
            }
            else
            {
                menu.gameObject.SetActive(true);
                ChangeInputScheme(InputScheme.Menu);
            }   
        }
    }

    public void SetCursorOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetCursorOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

}

public enum InputScheme
{
    Player,
    PinnyDialogue,
    Menu
}

