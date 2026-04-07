using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class Model : Singleton<Model>
{
    private PlayerData playerData;

    public bool isLoaded = false;

    public Model()
    {
    }

    public void Reset()
    {
        playerData = new PlayerData();
        var filePath = PlayerData.GetFilePath();
        var playerDataJson = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath, playerDataJson);
    }

    public void Save()
    {
        if (playerData == null || !isLoaded) return;
        playerData.localLastActiveTime = DateTime.Now.ToString();
        var filePath = PlayerData.GetFilePath();
        var playerDataJson = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath, playerDataJson);
    }

    public void Load()
    {
        var filePath = PlayerData.GetFilePath();
        var fileStream = File.Open(filePath, FileMode.OpenOrCreate);
        var sr = new StreamReader(fileStream);
        var playerDataJson = sr.ReadToEnd();
        sr.Close();
        fileStream.Close();

        playerData = JsonUtility.FromJson<PlayerData>(playerDataJson);
        if (playerData != null && string.IsNullOrEmpty(playerData.localFirstActiveTime))
        {
            playerData.localFirstActiveTime = DateTime.Now.ToString();
            playerData.localLastActiveTime = playerData.localFirstActiveTime;
        }

        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.localFirstActiveTime = DateTime.Now.ToString();
            playerData.localLastActiveTime = playerData.localFirstActiveTime;
            playerData.enterGameTimeCount = 0;
            playerData.coinCount = 0;
            playerData.gemCount = 0;
            playerData.curLevelId = 1;
        }

        foreach (var levelDataStatic in LevelDataSO.Instance.levelDataStatics)
        {
            LevelDataInfor levelDataInfor = playerData.gameData.levelDataInfors
                .Find(x => x.id == levelDataStatic.id);
            if (levelDataInfor == null)
            {
                levelDataInfor = new LevelDataInfor
                {
                    id = levelDataStatic.id,
                    stateLv = StateLv.Lock,
                    starCount = 0,
                    timeLimit = levelDataStatic.timeLimit,

                };
                if(levelDataInfor.id == 1)
                    levelDataInfor.stateLv = StateLv.UnLock;
                playerData.gameData.levelDataInfors.Add(levelDataInfor);
            }
            else
            {
                levelDataInfor.id = levelDataStatic.id;
                levelDataInfor.timeLimit = levelDataStatic.timeLimit;
            }
        }
        isLoaded = true;
        PlayerData.current = playerData;
    }

    public void ClearData()
    {
        var filePath = PlayerData.GetFilePath();
        var playerDataJson = string.Empty;
        File.WriteAllText(filePath, playerDataJson);
    }
}