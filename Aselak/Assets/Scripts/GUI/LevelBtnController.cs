using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtnController : MonoBehaviour {
    public GameObject scenesActionsGO;
    public Text buttonTxt;
    public GameObject AdsPanelPrefab;

    private ScenesActions _scenesActions;
    private LevelPresenter _levelPresenter;
    private LevelSO.Level _lvl;
    private LevelSO.Stage _stg;
    private Button _button;

    void Awake () {
        // Linking the button for short access
        _button = GetComponent<Button>();
        // Linking the scenes actions for short access;
        scenesActionsGO = GameObject.Find("SceneActions");
        _scenesActions = scenesActionsGO.GetComponent<ScenesActions>();
        _levelPresenter = scenesActionsGO.GetComponent<LevelPresenter>();
    }
    
    // This should set the buttons position passed as parameter.
    // Also store the Level and Stage info given the param.
    public void Build(LevelSO.Level lvl)
    {
        _lvl = lvl;
        buttonTxt.text = lvl.name;
        transform.Find("Score").gameObject.SetActive(false);
        AddListener();
    }

    public void Build(LevelSO.Level lvl, LevelSO.Stage stg)
    {
        _lvl = lvl;
        _stg = stg;
        //GetComponent<Image>().color = new Color(120, 65, 255, 1);
        buttonTxt.text = stg.name;
        for(int i = 0; i < GameManager.instance.playerScore.GetScore(lvl.id, stg.id); i++)
        {
            transform.Find("Score").GetChild(i).Find("Aselak").GetComponent<Image>().enabled = true;
        }
        AddListener();
    }

    // Sets the correct events due to the type of button
    // if is a level button it should call to set up the stages panel, change to it
    // if is a stage button it should call to hide the panel, then change the scene to the level
    void AddListener()
    {
        if(_stg == null)
        {
            _button.onClick.AddListener(() => { SetUpStagePanel(); });
        } else
        {
            _button.onClick.AddListener(() => { ChangeSceneToLevel(); });
        }
    }

    // Calls Level presenter with level data to set up the stage panel with level stages, then
    // then calls the scenes actions to swap panels between level and stage panels.
    void SetUpStagePanel()
    {
        if (GameManager.instance.playerLives > 0)
        {
            _levelPresenter.SetStages(
                _scenesActions.MenuChange("StagePanel").Panel,
                _lvl);
        }
        else
        {
            // else pop modal for ads viewer
            MenuManager._instance.ShowScreen(AdsPanelPrefab, forceHide: false);
            //AdsManager.ShowAd();
        }
    }

    // Closes the active panel and change the level scene gameplay
    void ChangeSceneToLevel()
    {
        if (GameManager.instance.playerLives > 0)
        {
            string sceneName = _lvl.sceneLabel + "-" + _stg.sceneLabel;
            _scenesActions.ChangeScene(sceneName, _lvl, _stg);
        }
        else
        {
            // else pop modal for ads viewer
            MenuManager._instance.ShowScreen(AdsPanelPrefab, forceHide: false);
            //AdsManager.ShowAd();
        }
    }
}
