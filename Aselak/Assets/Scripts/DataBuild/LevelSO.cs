using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LvlData.asset", menuName = "Levels/List", order = 1)]
public class LevelSO : ScriptableObject {
    public Level[] levels;

    [System.Serializable]
    public class Level
    {
        public int id;
        public string name;
        public string sceneLabel;
        public bool playable = false;
        public int nextLevelId = -1;

        public Stage[] stages;
    }

    [System.Serializable]
    public class Stage
    {
        public int id;
        public string name;
        public int ownerLevelId = -1;
        public int nextStageId = -1;

        public string sceneLabel;
        public bool playable = false;
        public float maxScore = 3f;

        public string cinematicSceneLabel;
    }
}
