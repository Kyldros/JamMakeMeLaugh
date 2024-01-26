using UnityEngine;


public interface IAudioPlayer
{
    abstract void PlayAudio(AudioClip audioClip, AudioSource audioSource);

    abstract public void StopAudio(AudioSource audioSource);
   
}
