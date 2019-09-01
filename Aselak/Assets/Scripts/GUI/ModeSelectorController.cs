using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModeSelectorController : MonoBehaviour {
    public Button explorationBtn;
    public Button directionalBtn;
    public Button burstBtn;
    public Button launchBtn;

    void Start () {
        // Adds Listeners to buttons to change the input mode of the spaceship
        explorationBtn.onClick.AddListener(() =>
        {
            if(GameManager.instance.player != null)
            {
                GameManager.instance.player.SetMode(ShipMode.Exploration);
                MenuManager._instance.BackScreen();
            }
        });

        directionalBtn.onClick.AddListener(() =>
        {
            if (GameManager.instance.player != null)
            {
                GameManager.instance.player.SetMode(ShipMode.Directional);
                MenuManager._instance.BackScreen();
            }
        });

        burstBtn.onClick.AddListener(() =>
        {
            if (GameManager.instance.player != null)
            {
                GameManager.instance.player.SetMode(ShipMode.Burst);
                MenuManager._instance.BackScreen();
            }
        });

        launchBtn.onClick.AddListener(() =>
        {
            if (GameManager.instance.player != null)
            {
                GameManager.instance.player.SetMode(ShipMode.Launch);
                MenuManager._instance.BackScreen();
            }
        });

    }
	
}
