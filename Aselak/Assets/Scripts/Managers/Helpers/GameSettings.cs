[System.Serializable]
public class GameSettings {
    public float musicVolume = 0f;
    public float efxVolume = 0f;
    public bool masterMute = false;
    public bool showTutorial = true;

    public GameSettings Clone()
    {
        GameSettings clone = new GameSettings();
        clone.musicVolume = musicVolume;
        clone.efxVolume = efxVolume;
        clone.masterMute = masterMute;
        clone.showTutorial = showTutorial;

        return clone;
    }

    public void Copy(GameSettings other)
    {
        musicVolume = other.musicVolume;
        efxVolume = other.efxVolume;
        masterMute = other.masterMute;
        showTutorial = other.showTutorial;
    }
}
