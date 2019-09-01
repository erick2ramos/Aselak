using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayScript : MonoBehaviour {

	public void Replay()
    {
        GameManager.instance.Replay();
    }

    public void NextStage()
    {
        GameManager.instance.NextStage();
    }

    public void Pause()
    {
        GameManager.instance.Pause();
    }

    public void Quit()
    {
        GameManager.instance.Quit();
    }
}
