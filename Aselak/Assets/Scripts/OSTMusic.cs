using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSTMusic : MonoBehaviour {
    public bool onStart = false;
    public AudioClip OSTrack;
    public bool stopCurrentMusic = false;

	// Use this for initialization
	void Start () {
        if (onStart)
        {
            PlayOST();
        }
	}
	
    void PlayOST()
    {
        if (!SoundManager.instance.playingMusic || 
            (stopCurrentMusic && (OSTrack.name != SoundManager.instance.musicSource.clip.name))
            )
        {
            SoundManager.instance.StopMusic();
            SoundManager.instance.PlayMusic(OSTrack, true);
        }
    }

    void StopOST()
    {
        SoundManager.instance.StopMusic(true);
    }
}
