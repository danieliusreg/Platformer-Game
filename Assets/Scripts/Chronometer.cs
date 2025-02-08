using UnityEngine;
using UnityEngine.UI; 

public class Chronometer : MonoBehaviour
{
    private float startTime; // Time when the game starts
    private bool isGameOver = false; // Flag to stop the timer when the game ends

    public Text timerText; // Reference to the Text UI element

    void Start()
    {
        startTime = Time.time; 
    }

    void Update()
    {
        if (!isGameOver)
        {
            float timeElapsed = Time.time - startTime; 

        
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);
            string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

            
            timerText.text = timeFormatted;
        }
    }

    
    public void EndGame()
    {
        isGameOver = true; 
    }
}
