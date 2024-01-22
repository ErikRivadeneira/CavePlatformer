using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateSingleton
{
    //Singleton Instance
    private readonly static GameStateSingleton _instance = new GameStateSingleton();

    // Current Player Lives
    private int currentLifes = 3;
    // Bool to check game over
    private bool isGameOver = false;
    // Bool to check game paused
    private bool isGamePaused = false;

    // Current timer count (not yet implemented)

    private GameStateSingleton() { }

    public static GameStateSingleton Instance
    {
        get { return _instance; }
    }

    public int getCurrentLives()
    {
        return currentLifes;
    }

    public bool getIsGameOver()
    {
        return isGameOver;
    }

    public bool getIsGamePaused()
    {
        return isGamePaused;
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
    }

    public void setCurrentLifes(int newLifeVal)
    {
        currentLifes = newLifeVal;
        isGameOver = currentLifes == 0 ? true : false;
    }
}
