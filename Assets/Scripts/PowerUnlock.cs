using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUnlock : MonoBehaviour
{
    public bool unlockDash = false;
    public bool unlockTPose = false;
    public bool unlockRagdoll = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            if(unlockDash)
                player.UnlockDash();
            if (unlockTPose)
                player.UnlockTPose();
            if (unlockRagdoll)
                player.UnlockRagdoll();
        }
    }
}
