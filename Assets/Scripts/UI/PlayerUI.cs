using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class PlayerUI : MonoBehaviour
{
    // Player UI GameObject
    [SerializeField] GameObject playerUI;
    // Text Field for player lifes
    [SerializeField] TextMeshProUGUI playerLifeText;
    // GameObject for GameOver
    [SerializeField] GameObject gameOverScreen;
    // Variables for stopwatch (Not implemented yet)
    float currentTime;
    [SerializeField] TextMeshProUGUI stopwatchText;

    //GameOver Event Fire Counter
    private int gOCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerLifeText.text = "x"+GameStateSingleton.Instance.getCurrentLives().ToString();
        currentTime = GameStateSingleton.Instance.getGameTime();
        TimeSpan lastGameTime = TimeSpan.FromSeconds(currentTime);
        stopwatchText.text = lastGameTime.ToString(@"mm\:ss\:fff");
    }

    private void Update()
    {
        UpdateStopwatch();
        if (GameStateSingleton.Instance.getIsGameOver() && gOCounter == 0)
        {
            GameOver();
            gOCounter++;
        }
    }

    void GameOver()
    {
        playerUI.SetActive(false);
        gameOverScreen.SetActive(true);
        Invoke(nameof(toCreditsScene), 5f);
    }

    void toCreditsScene()
    {
        // Send Player to Credits Scene;
        Debug.Log("Sent to credits");
    }

    void UpdateStopwatch()
    {
        if(!GameStateSingleton.Instance.getIsGameOver())
        {
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            stopwatchText.text = time.ToString(@"mm\:ss\:fff");
            GameStateSingleton.Instance.setGameTime(currentTime);
        }
    }
    
    void pauseGame()
    {
      
    } 
    
    void unpauseGame()
    {

    }
}
