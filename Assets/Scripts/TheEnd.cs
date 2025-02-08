using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    public AudioClip musicClip; // Assign the music clip in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Play the music if a clip is assigned
        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true; // Enable looping if needed
            audioSource.Play();
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0); 
    }
}
