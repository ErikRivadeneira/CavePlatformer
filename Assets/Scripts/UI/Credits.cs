using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private CanvasGroup AnnouncementCanvas;
    [SerializeField] private GameObject creditsObject;
    private bool fadingIn = true;
    private bool startFade = false;

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
