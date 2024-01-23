using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    // Player UI GameObject
    [SerializeField] GameObject playerUI;
    // Pause UI GameObject
    [SerializeField] GameObject pauseMenu;
    // Text Field for player lifes
    [SerializeField] TextMeshProUGUI playerLifeText;
    // GameObject for GameOver
    [SerializeField] GameObject gameOverScreen;
    // Variables for stopwatch (Not implemented yet)
    float currentTime;
    [SerializeField] TextMeshProUGUI stopwatchText;

    void Start()
    {
        playerLifeText.text = "x"+GameStateSingleton.Instance.getCurrentLives().ToString();
        currentTime = GameStateSingleton.Instance.getGameTime();
        TimeSpan lastGameTime = TimeSpan.FromSeconds(currentTime);
        stopwatchText.text = lastGameTime.ToString(@"mm\:ss\:fff");
    }

    private void Update()
    {
        checkPause();
        UpdateStopwatch();

        if (GameStateSingleton.Instance.getIsGameOver())
        {
            GameOver();
        }
    }

    /// <summary>
    /// Check pause/unpause input
    /// </summary>
    void checkPause()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameStateSingleton.Instance.getIsGamePaused())
        {
            pauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) && GameStateSingleton.Instance.getIsGamePaused())
        {
            unpauseGame();
        }
    }

    /// <summary>
    /// Start gameOver
    /// </summary>
    void GameOver()
    {
        playerUI.SetActive(false);
        gameOverScreen.SetActive(true);
        Invoke(nameof(toCreditsScene), 5f);
    }

    /// <summary>
    /// Send player to credits scene
    /// </summary>
    void toCreditsScene()
    {
        // Send Player to Credits Scene;
        SceneManager.LoadScene("Credits");
        Debug.Log("Sent to credits");
    }

    /// <summary>
    /// Update stopwatch in UI if game is not over or paused
    /// </summary>
    void UpdateStopwatch()
    {
        if(!GameStateSingleton.Instance.getIsGameOver() && !GameStateSingleton.Instance.getIsGamePaused())
        {
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            stopwatchText.text = time.ToString(@"mm\:ss\:fff");
            GameStateSingleton.Instance.setGameTime(currentTime);
        }
    }
    
    /// <summary>
    /// Pause game
    /// </summary>
    public void pauseGame()
    {
        GameStateSingleton.Instance.PauseGame();
        playerUI.SetActive(false);
        pauseMenu.SetActive(true);
    } 
    
    /// <summary>
    /// Unpause game
    /// </summary>
    public void unpauseGame()
    {
        GameStateSingleton.Instance.PauseGame();
        playerUI.SetActive(true);
        pauseMenu.SetActive(false);
    }
}
