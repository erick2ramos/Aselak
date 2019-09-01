using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public LevelSO.Level currentLevel;
    public LevelSO.Stage currentStage;
    public static LevelManager _instance;

    public LevelSO allLevels;

    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
            
        } else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveLevels(PlayerScore playerScore)
    {
        foreach(LevelSO.Level level in allLevels.levels)
        {
            if (playerScore.lastLevel == level.id || playerScore.hasLevel(level.id))
            {
                level.playable = true;
                foreach (LevelSO.Stage stage in level.stages)
                {
                    if(stage.id == playerScore.lastStage || playerScore.hasStage(level.id, stage.id))
                    {
                        stage.playable = true;
                    }
                }
            }
        }
    }

    public void ChangeLevel(string sceneName, LevelSO.Level lvl, LevelSO.Stage stg)
    {
        currentLevel = lvl;
        currentStage = stg;
        GameManager.instance.playerScore.lastLevel = lvl.id;
        GameManager.instance.playerScore.lastStage = stg.id;
        DataManager.data.Save();
        if (stg.cinematicSceneLabel != "" && stg.cinematicSceneLabel != null)
        {
            SceneManager.LoadScene(stg.cinematicSceneLabel);
        } else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void NextStage()
    {
        // Handle the next stage scene load and change
        if( currentStage.nextStageId != -1 )
        {
            foreach (LevelSO.Stage nextStage in currentLevel.stages)
            {
                if(nextStage.id == currentStage.nextStageId)
                {
                    nextStage.playable = true;
                    ChangeLevel(currentLevel.sceneLabel + "-" + nextStage.sceneLabel, currentLevel, nextStage);
                    return;
                }
            }
        } else if (currentLevel.nextLevelId != 1)
        {
            foreach (LevelSO.Level nextLevel in allLevels.levels)
            {
                if(nextLevel.id == currentLevel.nextLevelId)
                {
                    nextLevel.playable = true;
                    ChangeLevel(nextLevel.sceneLabel + "-" + nextLevel.stages[0].sceneLabel, nextLevel, nextLevel.stages[0]);
                    return;
                }
            }
        }
        else
        {
            ChangeLevel(currentLevel.sceneLabel + "-" + currentStage.sceneLabel, currentLevel, currentStage);
        }
    }
}
