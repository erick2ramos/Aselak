using System;
using UnityEngine;
using UnityEngine.UI;

public class AdsPanelController : MonoBehaviour {
    public Text timer;
    public Button watchAds;

	void Start () {
        watchAds.onClick.AddListener(delegate
        {
            AdsManager.ShowAd();
        });
	}
	
	void Update () {
        UpdateTimer();
	}
    
    public void UpdateTimer()
    {
        TimeSpan ts = GameManager.instance.recoverTimer.Subtract(DateTime.Now);
        timer.text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
    }
}
