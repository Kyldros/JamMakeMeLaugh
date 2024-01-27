using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerEffector3D : MonoBehaviour
{
    public Collider platformColliderToDisable;

    public void DisableCollider()
    {
        platformColliderToDisable.enabled = false;
    }

    public void EnableCollider()
    {
        platformColliderToDisable.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;

        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().RegisterPlatform(this);
        }

        if (direction.y < 0)
        {
            DisableCollider();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnableCollider();
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().UnregisterPlatform(this);
        }
    }

}


