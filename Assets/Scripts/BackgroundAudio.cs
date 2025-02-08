using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SetVolume(savedVolume);
            audioSource.Play();
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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

}
