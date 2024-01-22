using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject pauseMenu;

    /// <summary>
    /// Takes player to main menu
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Closes options panel from pause or main menu
    /// </summary>
    public void CloseOptions()
    {
        if(menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
        optionsPanel.SetActive(false);
    }

    /// <summary>
    /// Opens option panel from pause or main menu
    /// </summary>
    public void OpenOptions() 
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
        optionsPanel.SetActive(true);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Takes player to the first level
    /// </summary>
    public void StartGame()
    {
        GameStateSingleton.Instance.InitializeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
