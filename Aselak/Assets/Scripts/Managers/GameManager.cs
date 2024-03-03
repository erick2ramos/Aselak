using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public GameObject pauseScreenPrefab;
    public GameObject retryScreenPrefab;
    public bool paused = false;
    public DateTime suspendTimestamp;
    public GameObject AdsPanelPrefab;

    // Persistent data filled
    public PlayerScore playerScore;
    public int playerLives;
    // -----

    public BallScript player;
    public int maxPlayerLives = 3;
    public PreviousLaunch previousLaunch;

    public bool tutorialShown = false;

    private GameObject _pauseScreen;

    // Recovery Attributes
    public DateTime recoverTimer = DateTime.Now;
    private int recoverCooldown = 30;
    public bool playerDied;
    public bool adViewed;

    public void StoreLaunch(Vector3 direction, float burst)
    {
        previousLaunch = new PreviousLaunch(direction, burst);
    }

    // Use this for initialization
    void Awake () {
		if(instance == null)
        {
            playerScore = new PlayerScore();
            playerLives = maxPlayerLives;
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
	}

    
    public void ForceTimerLoad()
    {
        OnApplicationPause(false);
    }

    // Save on application pause
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // Store app pause time for comparison on app resume
            suspendTimestamp = DateTime.Now;
            DataManager.data.Save();
        } else if (DataManager.data != null)
        {
            // Load data including stored pause/stop timestamp
            DataManager.data.Load();
            // Caching time now
            DateTime dtBase = DateTime.Now;
            // Timespan passed between pause and resume
            TimeSpan ts = dtBase.Subtract(suspendTimestamp);
            // Locate the last recoverTimer start to get the time span spent recovering life
            // until the pause was triggered, that time span is added to the time spent on
            // pause to get the total traversed time recovering life
            DateTime startTime = recoverTimer.Subtract(new TimeSpan(0, recoverCooldown, 0));
            TimeSpan deltaTime = suspendTimestamp.Subtract(startTime);
            TimeSpan traversedTime = deltaTime + ts;
            int recover = ((int)traversedTime.TotalMinutes) / recoverCooldown;
            
            // If after restoring life the player has lives left to recover, we calculate
            // how much time is needed to restoring the next life. Otherwise there's no
            // need to calculate because the player has full lives
            if (playerLives + recover < maxPlayerLives)
            {
                TimeSpan deltaTs = traversedTime - new TimeSpan(0, recover * recoverCooldown,0);
                TimeSpan timeToNextLife = new TimeSpan(0, recoverCooldown, 0) - deltaTs;
                recoverTimer = dtBase.Add(timeToNextLife);
            }
            GainLives(recover);
        }
        
    }

    private void OnApplicationQuit()
    {
        if (playerDied)
        {
            LoseLives();
            playerDied = false;
        }

        suspendTimestamp = DateTime.Now;
        DataManager.data.Save();
        Debug.Log("Quit Save");
    }

    private void Update()
    {

        if (DateTime.Now > recoverTimer)
        {
            // Start recovery cooldown, set next time threshold 
            // then if time.now surpases the threshold add 1 live
            if (playerLives < maxPlayerLives)
            {
                GainLives();
            }
        }
    }

    public void GainLives(int amount = 1)
    {
        playerLives = Mathf.Min(maxPlayerLives, playerLives + amount);
        if(playerLives == maxPlayerLives)
        {
            recoverTimer = DateTime.Now;
        }
        if(playerLives < maxPlayerLives)
        {
            recoverTimer = DateTime.Now.AddMinutes(recoverCooldown);
        }
    }

    public void LoseLives(int amount = 1)
    {
        playerLives = Mathf.Max(playerLives - amount, 0);
        if(DateTime.Now > recoverTimer)
        {
            recoverTimer = DateTime.Now.AddMinutes(recoverCooldown);
        }
    }

    public void Replay()
    {
        if(paused)
            Pause();
        if(playerLives > 1 )
        {
            LoseLives();
            if (adViewed)
            {
                GainLives();
                adViewed = false;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Else show pop up with option to watch ads for lives refill
            MenuManager._instance.ShowScreen(AdsPanelPrefab, forceHide: false);
        }
    }

    public void NextStage()
    {
        if (paused)
            Pause();
        previousLaunch = null;
        LevelManager._instance.NextStage();
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0;
            MenuManager._instance.ShowScreen(pauseScreenPrefab, forceHide: false);
        } 
        else
        {
            Time.timeScale = 1;
            MenuManager._instance.BackScreen();
        }
    }

    public void Quit()
    {
        if (paused)
        {
            Pause();
        }

        if (playerDied)
        {
            LoseLives();
            playerDied = false;
        }
        if (adViewed)
        {
            GainLives();
            adViewed = false;
        }
        previousLaunch = null;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver(int finalScore, bool died)
    {
        playerDied = died;
        StartCoroutine(WaitFor(1, () =>
        {
            // Setup and show success / fail menu
            PanelMenu pm = MenuManager._instance.ShowScreen(retryScreenPrefab);
            pm.Panel.gameObject.GetComponent<WinMenu>().SetupMenu(finalScore, 3, died);

            // Animate stars after the menu has been shown
            StartCoroutine(WaitFor(0.5f, () =>
            {
                pm.Panel.gameObject.GetComponent<WinMenu>().GrantStars(finalScore);
            }));
        }));
    }

    IEnumerator WaitFor(float secs, UnityAction callback)
    {
        yield return new WaitForSecondsRealtime(secs);
        callback();
    }
}