using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] public Slider sfxSlider;
    [SerializeField] public Slider bgmSlider; 

    private void Awake()
    {
        InitializeValuesAndAudio();
    }

    // Start is called before the first frame update
    void Start()
    {
        bgmSlider.onValueChanged.AddListener((value) => UpdateBgmSourceVolume(value));
        sfxSlider.onValueChanged.AddListener((value) => UpdateSfxSourceVolume(value));
    }

    /// <summary>
    /// Checks if Unity PlayerPrefs has Sfx and/or Bgm volume values and updates if true
    /// </summary>
    void InitializeValuesAndAudio()
    {
        if (PlayerPrefs.HasKey("BGMVol"))
        {
            float savedBgmVolume = PlayerPrefs.GetFloat("BGMVol");
            bgmSlider.value = savedBgmVolume;
            UpdateBgmSourceVolume(savedBgmVolume);
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            float savedSfxVolume = PlayerPrefs.GetFloat("SFXVol");
            sfxSlider.value = savedSfxVolume;
            UpdateSfxSourceVolume(savedSfxVolume);
        }
    }

    /// <summary>
    /// Searches all objects with BGM tag and updates volume value on audio source
    /// </summary>
    /// <param name="value">float between 0 to 1</param>
    void UpdateBgmSourceVolume(float value)
    {
        var BGMSources = GameObject.FindGameObjectsWithTag("BGM");
        foreach (GameObject source in BGMSources) 
        {
            source.GetComponent<AudioSource>().volume = value;
        }
        PlayerPrefs.SetFloat("BGMVol", value);
    }

    /// <summary>
    /// Searches all objects with SFX tag and updates volume value on audio source
    /// </summary>
    /// <param name="value">float between 0 to 1</param>
    void UpdateSfxSourceVolume(float value)
    {
        var SfxSources = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject source in SfxSources)
        {
            source.GetComponent<AudioSource>().volume = value;
        }
        PlayerPrefs.SetFloat("SFXVol", value);
    }
}
