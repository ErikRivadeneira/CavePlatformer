using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Text Field for player lifes
    [SerializeField] TextMeshProUGUI playerLifeText;
    // GameObject for GameOver
    [SerializeField] GameObject gameOverScreen;
    // Variables for stopwatch (Not implemented yet)

    //GameOver Event Fire Counter
    private int gOCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerLifeText.text = "x"+GameStateSingleton.Instance.getCurrentLives().ToString();
    }

    private void Update()
    {

        if (GameStateSingleton.Instance.getIsGameOver() && gOCounter == 0)
        {
            GameOver();
            gOCounter++;
        }
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        Invoke(nameof(toCreditsScene), 5f);
    }

    void toCreditsScene()
    {
        // Send Player to Credits Scene;
        Debug.Log("Sent to credits");
    }
}
