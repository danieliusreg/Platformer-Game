using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // For scene management

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public Slider musicVolumeSlider;  // Slider for background music
    public Text musicVolumeText;
    public Slider sfxVolumeSlider;    // Slider for sound effects
    public Text sfxVolumeText;
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    public BackgroundMusic backgroundMusicController; // Reference to BackgroundMusic script
    public SoundEffects soundEffectsController;       // Reference to SoundEffects script

    public Button backToMainMenuButton;  // New button to go back to the main menu

    // Reference to the second canvas (the one with the jump counter)
    public GameObject secondCanvas;

    private bool isMenuOpen = false;

    void Start()
    {
        // Make sure game is not paused when starting
        Time.timeScale = 1f;

        // Initialize menu components
        musicVolumeSlider.value = backgroundMusicController.GetVolume();
        musicVolumeText.text = "MUSIC: " + Mathf.RoundToInt(musicVolumeSlider.value * 100);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        sfxVolumeSlider.value = soundEffectsController.GetVolume();
        sfxVolumeText.text = "SFX: " + Mathf.RoundToInt(sfxVolumeSlider.value * 100);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        brightnessSlider.value = savedBrightness;
        UpdateBrightness(savedBrightness);
        brightnessSlider.onValueChanged.AddListener(UpdateBrightness);

        backToMainMenuButton.onClick.AddListener(BackToMainMenu);

        menuPanel.SetActive(false);
        secondCanvas.SetActive(true); // Make sure second canvas is enabled when game starts
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);

        // Hide or show the second canvas based on whether the menu is open or closed
        secondCanvas.SetActive(!isMenuOpen);

        // Pause or resume the game based on the menu state
        if (isMenuOpen)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    void OnMusicVolumeChanged(float value)
    {
        backgroundMusicController.SetVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Save the changes immediately
        musicVolumeText.text = "MUSIC: " + Mathf.RoundToInt(value * 100);
    }

    void OnSFXVolumeChanged(float value)
    {
        soundEffectsController.SetVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Save the changes immediately
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

    // New method to load the main scene
    void BackToMainMenu()
    {
        SceneManager.LoadScene(0);  // Load the scene with index 0 (main scene)
    }
}
