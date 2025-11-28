using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audiosource;
    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();  
    }
    public void PlayAudio(AudioClip clip)
    {
        audiosource.PlayOneShot(clip);
    }
}
