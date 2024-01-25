using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.takeDamage(damage);
        }
    }
}
