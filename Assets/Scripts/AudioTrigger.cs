using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
   public AudioManager audioManager;
   public AudioClip musicClip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
        {
            audioManager.PlayMusic(musicClip);
        }

        
    }


}
