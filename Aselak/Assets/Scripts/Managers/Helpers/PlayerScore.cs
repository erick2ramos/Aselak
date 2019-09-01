using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore{
    [System.Serializable]
    public class PSTriplet
    {
        public int level;
        public int stage;
        public int score;

        public PSTriplet(int l, int s, int scr)
        {
            level = l;
            stage = s;
            score = scr;
        }
    }

    public List<PSTriplet> buckets;
    public int lastLevel;
    public int lastStage;

    public PlayerScore()
    {
        buckets = new List<PSTriplet>();
    }

    private PSTriplet SearchFor(int level, int stage)
    {
        foreach(PSTriplet triplet in buckets)
        {
            if(triplet.level == level && triplet.stage == stage)
            {
                return triplet;
            }
        }
        return null;
    }

    public int GetScore(int levelId, int stageId)
    {
        PSTriplet triplet = SearchFor(levelId, stageId);
        if ( triplet != null)
        {
            return triplet.score;
        }
        return -1;
    }
    
    public bool hasLevel(int levelId)
    {
        foreach (PSTriplet triplet in buckets)
        {
            if (triplet.level == levelId)
                return true;
        }
        return false;
    }

    public bool hasStage(int levelId, int stageId)
    {
        foreach (PSTriplet triplet in buckets)
        {
            if (triplet.level == levelId && triplet.stage == stageId)
            {
                return true;
            }
        }
        return false;
    }

    public void SetScore(int levelId, int stageId, int score)
    {
        PSTriplet triplet = SearchFor(levelId, stageId);
        if (triplet == null)
        {
            triplet = new PSTriplet(levelId, stageId, score);
            buckets.Add(triplet);
        } else
        {
            int previousScore = triplet.score;
            triplet.score = Mathf.Max(score, previousScore);
        }
    }
}
