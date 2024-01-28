using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
  
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == GameManager.Instance.player.GetGroundLayer())
        GameManager.Instance.player.isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == GameManager.Instance.player.GetGroundLayer())
            GameManager.Instance.player.isGrounded = false;
    }
}

