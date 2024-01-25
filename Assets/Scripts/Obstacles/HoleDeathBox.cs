using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDeathBox : MonoBehaviour
{
    public Transform teleportDestination;
    
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.transform.position=teleportDestination.position;
        }
    }
}
