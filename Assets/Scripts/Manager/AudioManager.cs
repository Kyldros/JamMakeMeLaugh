using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private AudioSource audioSource;
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            GameObject newObject = Instantiate(new GameObject());
            AudioSource source = newObject.AddComponent<AudioSource>();
            source.clip = audioClip;
            source.Play();
            Destroy(newObject, source.clip.length);
        }
    }
    public void PlayMusic(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void StopAudio(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
