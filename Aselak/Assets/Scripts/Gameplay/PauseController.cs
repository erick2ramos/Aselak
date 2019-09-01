using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour {
    public GameObject pauseScreen;
    //Must handle go to pause screen in form of an Event
    void Awake()
    {

    }

    public void Pause()
    {
        GameManager.instance.Pause();
    }
}
