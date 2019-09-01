using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public static class AdsManager {
    public static void ShowAd()
    {
        //if (Advertisement.IsReady())
        //{
        //    ShowOptions so = new ShowOptions();
        //    so.resultCallback = Callback;
        //    Advertisement.Show(so);
        //}
    }

    //public static void Callback(ShowResult sr)
    //{
    //    switch (sr)
    //    {
    //        case ShowResult.Skipped:
    //            {
    //                Debug.Log("AdsCallback::Skipped");
    //                break;
    //            }
    //        case ShowResult.Failed:
    //            {
    //                Debug.Log("AdsCallback::Failed");
    //                break;
    //            }
    //        case ShowResult.Finished:
    //            {
    //                Debug.Log("AdsCallback::Finished");
    //                GameManager.instance.GainLives(GameManager.instance.maxPlayerLives);
    //                GameManager.instance.adViewed = true;
    //                MenuManager._instance.BackScreen();
    //                break;
    //            }
    //    }
    //}
}