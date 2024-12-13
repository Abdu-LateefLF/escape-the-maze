using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer gameMixer;
    public AudioMixer musicMixer;

    public Dropdown qualityDropdown;
    public Slider qualityScaleSlider;

    public Slider sensXSlider;
    public Slider sensYSlider;

    public Slider musicVolumeSlider;
    public Slider gameVolumeSlider;

    public static float sensX = 40f;
    public static float sensY = 30f;

    void Start()
    {
        GetValuesFromSave();

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        //qualityScaleSlider.value = QualitySettings.resolutionScalingFixedDPIFactor;

        sensXSlider.value = sensX;
        sensYSlider.value = sensY;

        float gameVolume;
        gameMixer.GetFloat("volume", out gameVolume);
        gameVolumeSlider.value = Mathf.Pow(10, gameVolume / 20);

        float musicVolume;
        musicMixer.GetFloat("volume", out musicVolume);
        musicVolumeSlider.value = Mathf.Pow(10, musicVolume / 20);
    }

    void GetValuesFromSave()
    {
        if (PlayerPrefs.HasKey("GameVolume"))
            SetGameVolume(PlayerPrefs.GetFloat("GameVolume"));

        if (PlayerPrefs.HasKey("MusicVolume"))
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));

        if (PlayerPrefs.HasKey("QualityLevel"))
            SetQuality(PlayerPrefs.GetInt("QualityLevel"));

        //if (PlayerPrefs.HasKey("ResolutionScale"))
            //SetQualityScale(PlayerPrefs.GetFloat("ResolutionScale"));

        if (PlayerPrefs.HasKey("HorizontalSensitivity"))
            SetHoriSensitivity(PlayerPrefs.GetFloat("HorizontalSensitivity"));
        else
            SetHoriSensitivity(28f);

        if (PlayerPrefs.HasKey("VerticalSensitivity"))
            SetVertSensitivity(PlayerPrefs.GetFloat("VerticalSensitivity"));
        else
            SetVertSensitivity(12f);
    }

    public void SetGameVolume(float volume)
    {
        gameMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("GameVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetQualityScale(float scale)
    {
       // QualitySettings.resolutionScalingFixedDPIFactor = scale;
       // PlayerPrefs.SetFloat("ResolutionScale", scale);
    }

    public void SetHoriSensitivity(float sensitivity)
    {
        sensX = sensitivity;
        PlayerPrefs.SetFloat("HorizontalSensitivity", sensitivity);
    }

    public void SetVertSensitivity(float sensitivity)
    {
        sensY = sensitivity;
        PlayerPrefs.SetFloat("VerticalSensitivity", sensitivity);
    }
}
