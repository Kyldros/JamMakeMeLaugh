using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public PlayerInput playerInput;

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

    public void OpenClippi()
    {

    }

    public void ChangeInputScheme(InputScheme scheme)
    {
        playerInput.SwitchCurrentActionMap(scheme.ToString());
    }

}

public enum InputScheme
{
    Player,
    PinnyDialogue,
    Menu
}

