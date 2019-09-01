using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPresenter : MonoBehaviour {
    public LevelSO lvlData;
    public GameObject levelSelectorPrefab;

    private Text[] titles = new Text[2];

    public void ShowLevels()
    {
        GameObject lvlPanel = GameObject.Find("Canvas/LevelPanel");
        SetLevels(MenuManager._instance.ShowScreen(lvlPanel).Panel);
    }


    public void SetLevels(RectTransform panel)
    {
        titles[0] = panel.Find("Title").GetComponent<Text>();
        for (int i = 0; i < lvlData.levels.Length; i++)
        {
            if (lvlData.levels[i].playable)
            {
                GameObject go = Instantiate(levelSelectorPrefab);
                go.GetComponent<RectTransform>().SetParent(panel.Find("Panel"), false);
                LevelBtnController lbc = go.GetComponent<LevelBtnController>();
                lbc.Build(lvlData.levels[i]);
            }
        }
    }

    public void SetStages(RectTransform panel, LevelSO.Level level)
    {
        titles[1] = panel.Find("Title").GetComponent<Text>();
        titles[1].text = level.name + " Sectors";
        for (int i = 0; i < level.stages.Length; i++)
        {
            if (level.stages[i].playable)
            {
                GameObject go = Instantiate(levelSelectorPrefab);
                LevelBtnController lbc = go.GetComponent<LevelBtnController>();
                lbc.Build(level, level.stages[i]);
                go.GetComponent<RectTransform>().SetParent(panel.Find("Panel"), false);
            }
        }
    }
}
