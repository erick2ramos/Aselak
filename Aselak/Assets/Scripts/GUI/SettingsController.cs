using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsController : MonoBehaviour {
    public Button backButton;
    public Slider efxSlider;
    public Slider musicSlider;
    public Toggle muteToggle;
    public Toggle tutorialToggle;

    public AudioClip audioDemo;
    private bool playingSound = false;

    private void OnEnable()
    {

        //Initialize values according to the game settings values in settings manage
        SetMusicSlider(SettingsManager.instance.gameSettings.musicVolume);
        SetEfxSlider(SettingsManager.instance.gameSettings.efxVolume);
        SetMuteToggle(SettingsManager.instance.gameSettings.masterMute);
        SetTutorialToggle(SettingsManager.instance.gameSettings.showTutorial);

        // Apply listeners to controllers
        backButton.onClick.AddListener(Back);
        efxSlider.onValueChanged.AddListener(delegate { OnEfxSliderChange(); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChange(); });
        muteToggle.onValueChanged.AddListener(delegate { OnMuteToggle(); });
        tutorialToggle.onValueChanged.AddListener(delegate { OnTutorialToggle(); });


        // Button only interactable if change has been made
    }

    void OnApply()
    {
        SettingsManager.instance.SaveSettings();
    }

    void OnMusicSliderChange()
    {
        SetMusicSlider(musicSlider.value);
        OnApply();
    }

    void OnEfxSliderChange()
    {
        SetEfxSlider(efxSlider.value);
        if (!playingSound)
        {
            playingSound = true;
            SoundManager.instance.PlaySingle(audioDemo);
            StartCoroutine(WaitFor(1.5f, () =>
            {
                playingSound = false;
            }));
        }
        OnApply();
    }

    void OnMuteToggle()
    {
        SetMuteToggle(muteToggle.isOn);
        OnApply();
    }

    void OnTutorialToggle()
    {
        SetTutorialToggle(tutorialToggle.isOn);
        OnApply();
    }

    void Back()
    {
        SettingsManager.instance.LoadSettings();
        MenuManager._instance.BackScreen();
    }

    void SetMusicSlider(float value)
    {
        SettingsManager.instance.musicVolume = musicSlider.value = value;
    }

    void SetEfxSlider(float value)
    {
        SettingsManager.instance.efxVolume = efxSlider.value = value;
    }

    void SetMuteToggle(bool value)
    {
        SettingsManager.instance.masterVolume = muteToggle.isOn = value;
    }

    void SetTutorialToggle(bool value)
    {
        SettingsManager.instance.tutorialEnabled = tutorialToggle.isOn = value;
    }

    IEnumerator WaitFor(float secs, UnityAction callback)
    {
        yield return new WaitForSeconds(secs);
        callback();
    }
}
