using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonSelection : MonoBehaviour
{
    public GameObject[] buttons;
    public int currentIndex { get; private set; } = 0;

    public void SelectButton(int newIndex)
    {
        newIndex = Mathf.Clamp(newIndex, 0, buttons.Length - 1);

        EventSystem.current.SetSelectedGameObject(buttons[newIndex]);

        currentIndex = newIndex;
    }
}
