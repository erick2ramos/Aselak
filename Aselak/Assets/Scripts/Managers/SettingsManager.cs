using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager instance;
    public GameSettings gameSettings;
    
    public bool showTutorial;

    public bool tutorialEnabled
    {
        set
        {
            showTutorial = value;
            gameSettings.showTutorial = value;
        }
    }

    public float efxVolume {
        set {
            gameSettings.efxVolume = value;
            SoundManager.instance.SetEfxVolume(value);
        }
    }

    public float musicVolume
    {
        set
        {
            gameSettings.musicVolume = value;
            SoundManager.instance.SetMusicVolume(value);
        }
    }

    public bool masterVolume
    {
        set
        {
            gameSettings.masterMute = value;
            SoundManager.instance.SetMuteMaster(value);
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            gameSettings = new GameSettings();
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        if(File.Exists(Application.persistentDataPath + "/game-settings.json"))
        {
            gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/game-settings.json"));
            musicVolume = gameSettings.musicVolume;
            efxVolume = gameSettings.efxVolume;
            masterVolume = gameSettings.masterMute;
            showTutorial = gameSettings.showTutorial;
        }
    }

    public void SaveSettings()
    {
        string jsonGameSettings = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/game-settings.json", jsonGameSettings);
    }
}
