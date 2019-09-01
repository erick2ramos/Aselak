using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour {
    public AudioSource efxSource;
    public AudioSource musicSource;
    public AudioMixer mainMixer;
    public bool playingMusic = false;

    public static SoundManager instance = null;
	// Use this for initialization
	void Awake () {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    public void SetMusicVolume(float dbs)
    {
        mainMixer.SetFloat("vMusic", dbs);
    }

    public void SetEfxVolume(float dbs)
    {
        mainMixer.SetFloat("vSFX", dbs);
    }

    public void SetMuteMaster(bool mute)
    {
        mainMixer.SetFloat("vMaster", mute ? -80 : 0);
    }

    public void PlaySingle(AudioClip clip, float volume = 1, float pitch = 1 )
    {
        efxSource.clip = clip;
        //efxSource.volume = volume;
        efxSource.pitch = pitch;
        efxSource.Play();
    }

    public void PlayMusic(AudioClip ost, bool fade = false, UnityAction callback = null)
    {
        musicSource.clip = ost;
        if (fade)
            StartCoroutine(FadeIn(musicSource, 0.3f, callback));
        else
            musicSource.Play();
        playingMusic = true;
    }

    public void StopMusic(bool fade = false, UnityAction callback = null)
    {
        if (fade)
           StartCoroutine(FadeOut(musicSource, 0.25f, callback));
        else
            musicSource.Stop();
        playingMusic = false;
    }

    IEnumerator FadeOut(AudioSource src, float rate, UnityAction callback = null)
    {
        float origVol = src.volume;
        float delta = 32;

        do
        {
            src.volume = Mathf.Lerp(src.volume, 0f, 0.03f);
            delta -= 1;
            yield return new WaitForEndOfFrame();
        } while (delta > 0);
        if(callback != null)
        {
            callback();
        }
        src.Stop();
        src.volume = origVol;
    }

    IEnumerator FadeIn(AudioSource src, float rate, UnityAction callback = null)
    {
        float origVol = src.volume;
        float delta = 32;
        src.volume = 0f;
        src.Play();
        do
        {
            src.volume = Mathf.Lerp(src.volume, origVol, rate);
            delta -= 1;
            yield return new WaitForEndOfFrame();
        } while (delta > 0);
        if (callback != null)
        {
            callback();
        }
        src.volume = origVol;
    }
}
