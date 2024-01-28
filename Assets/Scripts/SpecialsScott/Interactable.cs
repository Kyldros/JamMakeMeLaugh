using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent Action;

    private void OnTriggerEnter(Collider other)
    {
        Action.Invoke();
    }

    public void DeactivateObj(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ActivateObj(GameObject obj)
    {
        obj.SetActive(true);
    }
}
