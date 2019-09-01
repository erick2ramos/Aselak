using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public void ShowTutorial(bool show)
    {
        SettingsManager.instance.tutorialEnabled = !show;
        SettingsManager.instance.SaveSettings();
    }
}
