using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip runSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SetVolume(savedVolume);
    }

    public void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public void PlayLandSound()
    {
        if (landSound != null)
        {
            audioSource.PlayOneShot(landSound);
        }
    }

    public void PlayRunSound()
    {
        if (runSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(runSound);
        }
    }

    public void StopRunSound()
    {
        if (audioSource.isPlaying && audioSource.clip == runSound)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }
}
