using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Actions for scene and menu flux
public class ScenesActions : MonoBehaviour {
    public GameObject _activeScreen;

    void Start()
    {
        if(_activeScreen == null)
        {
            _activeScreen = GameObject.Find("Canvas/MainMenuPanel");
        }
        if(_activeScreen != null)
        {
            MenuManager._instance.ShowScreen(_activeScreen);
        }
    }

    public void Play()
    {
        Animator anim = GameObject.Find("Main Camera").GetComponent<Animator>();
        MenuManager._instance.BackScreen(() => {
            anim.Play("MoveCamera", anim.GetLayerIndex("SecondPosition"));
        });
        StartCoroutine(WaitFor(2.7f, () => {
            GetComponent<LevelPresenter>().ShowLevels();
        }));
    }

    public PanelMenu MenuChange(string scr, UnityAction listener = null)
    {
        _activeScreen = GameObject.Find("Canvas/" + scr);
        return MenuManager._instance.ShowScreen(_activeScreen, listener);
    }

    public void ScreenChange(string scr)
    {
        _activeScreen = GameObject.Find("Canvas/"+scr);
        MenuManager._instance.ShowScreen(_activeScreen);
    }

    public void Back()
    {
        MenuManager._instance.BackScreen();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(string sceneName, LevelSO.Level lvl, LevelSO.Stage stg)
    {
        SoundManager.instance.StopMusic(true, () =>
        {
            LevelManager._instance.ChangeLevel(sceneName, lvl, stg);
        });
    }

    public void BackToMainMenu()
    {
        Animator anim = GameObject.Find("Main Camera").GetComponent<Animator>();
        MenuManager._instance.BackScreen(() => {
            anim.Play("MoveInverse");
        });
        StartCoroutine(WaitFor(2.7f, () => {
            MenuManager._instance.ShowScreen(GameObject.Find("Canvas/MainMenuPanel"));
        }));
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator WaitFor(float seconds, Action callback = null)
    {
        yield return new WaitForSecondsRealtime(seconds);

        if (callback != null)
            callback();
    }
}
