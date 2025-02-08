using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;

    public Slider musicVolumeSlider;  // Slider for background music
    public Text musicVolumeText;
    public Slider sfxVolumeSlider;    // Slider for sound effects
    public Text sfxVolumeText;

    public Slider brightnessSlider;
    public Image brightnessOverlay;

    public BackgroundMusic backgroundMusicController; // Reference to BackgroundMusic script
    public SoundEffects soundEffectsController;       // Reference to SoundEffects script

    void Start()
    {
        settingsPanel.SetActive(false);

        // Load saved volumes
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.value = savedMusicVolume;
        musicVolumeText.text = "MUSIC: " + Mathf.RoundToInt(savedMusicVolume * 100);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxVolumeSlider.value = savedSFXVolume;
        sfxVolumeText.text = "SFX: " + Mathf.RoundToInt(savedSFXVolume * 100);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        // Initialize brightness slider
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        brightnessSlider.value = savedBrightness;
        UpdateBrightness(savedBrightness);
        brightnessSlider.onValueChanged.AddListener(UpdateBrightness);

        // Set the volume of background music based on saved value
        backgroundMusicController.SetVolume(savedMusicVolume);  // Set the background music volume immediately

        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1); // Replace with your actual game scene index or name
    }

    public void QuitGame()
    {
        Application.Quit(); // Quits the application (only works in a built version)
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    void OnMusicVolumeChanged(float value)
    {
        backgroundMusicController.SetVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Save immediately
        musicVolumeText.text = "MUSIC: " + Mathf.RoundToInt(value * 100);
    }

    void OnSFXVolumeChanged(float value)
    {
        soundEffectsController.SetVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Save immediately
        sfxVolumeText.text = "SFX: " + Mathf.RoundToInt(value * 100);
    }

    void UpdateBrightness(float value)
    {
        float adjustedValue = Mathf.Max(value, 0.1f);
        if (brightnessOverlay != null)
        {
            Color overlayColor = brightnessOverlay.color;
            overlayColor.a = 1 - adjustedValue;
            brightnessOverlay.color = overlayColor;
        }
        PlayerPrefs.SetFloat("Brightness", adjustedValue);
    }

    void Update()
    {
        // Close the settings panel when the Escape key is pressed
        if (settingsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }
}
