using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScript : MonoBehaviour {
    public GameObject gameManager;
    public GameObject soundManager;
    public GameObject settingsManager;
    public GameObject dataManager;

    // Use this for initialization
    void Awake () {
        if (GameManager.instance == null)
            Instantiate(gameManager);
        if (SoundManager.instance == null)
            Instantiate(soundManager);
        if (SettingsManager.instance == null)
            Instantiate(settingsManager);
        if (DataManager.data == null)
            Instantiate(dataManager);
    }

    public static void LoadLevel(int world, int lvl)
    {
        SceneManager.LoadScene("Level-" + world.ToString() + "-" + lvl.ToString());
    }
}
