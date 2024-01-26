using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject pinny;
    public GameObject menu;

    public List<TriggerWall> wallList = new();

    InputScheme playScheme = InputScheme.Player;

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

    public void TriggerWalls(bool value)
    {
        foreach (TriggerWall wall in wallList) 
        { 
            wall.gameObject.GetComponent<Collider>().enabled = value;
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
        if (scheme != InputScheme.Menu)
            playScheme = scheme;
    }
    public void OpenMenu()
    {
        if (menu != null && !menu.gameObject.activeInHierarchy)
        {
            menu.gameObject.SetActive(true);
            ChangeInputScheme(InputScheme.Menu);
            SetCursorOn();
        }

    }
    public void CloseMenu()
    {
        if (menu != null && menu.gameObject.activeInHierarchy) 
        { 
            menu.gameObject.SetActive(false);
            ChangeInputScheme(playScheme);
            SetCursorOff();
            SetIputActionState(true);
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


    private void Start()
    {
        if (playerInput != null)
            ChangeInputScheme(InputScheme.Menu);
        SetCursorOn();
        SetIputActionState(false);
    }

    private void SetIputActionState(bool active)
    {
        InputAction actionToDisable = playerInput.actions.FindAction("Menu");

        if (actionToDisable != null)
        {
            if(active)
                actionToDisable.Enable();
            else
                actionToDisable.Disable();
        }
    }
}

public enum InputScheme
{
    Player,
    PinnyDialogue,
    Menu
}

