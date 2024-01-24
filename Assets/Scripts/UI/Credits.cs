using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private CanvasGroup AnnouncementCanvas;
    [SerializeField] private GameObject creditsObject;
    [SerializeField] private TextMeshProUGUI winAnnounceText;
    [SerializeField] private TextMeshProUGUI runtimeText;
    [SerializeField] private Image bgImage;
    [SerializeField] private Sprite winImage;
    [SerializeField] private Sprite loseimage;
    private bool fadingIn = true;
    private bool startFade = false;

    private void Start()
    {
        if (GameStateSingleton.Instance.getGameWon())
        {
            winAnnounceText.text = "Congratulations! You escaped the cave!";
            bgImage.sprite = winImage;
        }
        else
        {
            winAnnounceText.text = "You have become nourishment for the cave...";
            bgImage.sprite = loseimage;
        }

        float currentTime = GameStateSingleton.Instance.getGameTime();
        TimeSpan lastGameTime = TimeSpan.FromSeconds(currentTime);
        runtimeText.text = lastGameTime.ToString(@"mm\:ss\:fff");
    }

    // Update is called once per frame
    void Update()
    {
        if(startFade)
        {
            ControlFade();
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AnnounceFadeIn()
    {
        if(AnnouncementCanvas.alpha < 1f)
        {
            AnnouncementCanvas.alpha += Time.deltaTime/2;
        }

        if(AnnouncementCanvas.alpha == 1f)
        {
            fadingIn = false;
        }
    }

    void AnnounceFadeOut() 
    {
        if(AnnouncementCanvas.alpha > 0f)
        {
            AnnouncementCanvas.alpha -= Time.deltaTime/2;
        }

        if(AnnouncementCanvas.alpha == 0f)
        {
            fadingIn = true;
        }
    }

    void ControlFade()
    {
        if(fadingIn)
        {
            AnnounceFadeIn();
        }
        else
        {
            AnnounceFadeOut();
        }
    }

    public void StartToFade()
    {
        startFade = true;
        creditsObject.SetActive(false);
    }
}
