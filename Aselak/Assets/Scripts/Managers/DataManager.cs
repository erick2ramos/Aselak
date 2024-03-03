using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {
    public static DataManager data;

    [Serializable]
    class PersistentData
    {
        public PlayerScore playerScore;
        public int shipLives;
        public string pauseDate;
        public string cooldownLeft;

        public PersistentData()
        {
            playerScore = new PlayerScore();
        }
    }

    [SerializeField]
    private PersistentData p_Data;

    [SerializeField]
    private string DATA_PATH;
	// Use this for initialization
	void Start () {
		if(data == null)
        {
            DATA_PATH = Application.persistentDataPath + "/save-info.dat";
            data = this;
            data.Load();
            GameManager.instance.ForceTimerLoad();
        } else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

    private void OnDisable()
    {
        data.Save();
    }

    //Save data to a file
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(DATA_PATH);
        p_Data = new PersistentData();
        p_Data.shipLives = GameManager.instance.playerLives;
        p_Data.playerScore = GameManager.instance.playerScore;
        p_Data.cooldownLeft = GameManager.instance.recoverTimer.ToString("yyyy-MM-ddTHH:mm:sszzz");
        p_Data.pauseDate = GameManager.instance.suspendTimestamp.ToString("yyyy-MM-ddTHH:mm:sszzz");
        bf.Serialize(file, p_Data);
        file.Close();
    }

    //Load data from file
    public void Load()
    {
        if(File.Exists(DATA_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(DATA_PATH, FileMode.Open);
            p_Data = (PersistentData)bf.Deserialize(file);
            GameManager.instance.playerScore = p_Data.playerScore;
            GameManager.instance.playerLives = p_Data.shipLives;
            var cooldownLeftDateTime = DateTime.Parse(p_Data.cooldownLeft);
            try
            {
                cooldownLeftDateTime.Subtract(TimeSpan.FromMinutes(30));
            } 
            catch (System.Exception ex)
            {
                cooldownLeftDateTime = DateTime.Now;
            }
            
            GameManager.instance.recoverTimer = cooldownLeftDateTime;
            GameManager.instance.suspendTimestamp = DateTime.Parse(p_Data.pauseDate);
            LevelManager._instance.SetActiveLevels(p_Data.playerScore);
            file.Close();
        }
    }
}
