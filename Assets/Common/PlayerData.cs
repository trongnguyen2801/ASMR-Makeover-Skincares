using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
    #region StaticConfig

#if UNITY_EDITOR
    private static readonly string directory = "Assets/";
#else
    private static readonly string directory = Application.persistentDataPath;
#endif
    private static string playerDataFileName = "/playerdata.txt";
    public static string GetFilePath()
    {
        return directory + playerDataFileName;
    }

    #endregion
    public static PlayerData current;
    public LanguageType languageType = LanguageType.ENGLISH;
    public string localFirstActiveTime;
    public string localLastActiveTime;
    public int enterGameTimeCount;
    public int coinCount;
    public int gemCount;
    public bool noAds = false;
    public string removeAds1dExpiredTime;
    public int curLevelId = 1;

    public GameData gameData = new GameData();
    public GameSetting gameSetting = new GameSetting();
    public TempData tempData = new TempData();

    public void AddGem(int count)
    {
        gemCount += count;
        if (gemCount < 0)
            gemCount = 0;
        if (count > 0)
        {
            tempData.earnedGemCount += count;
        }
    }

    public void AddCoin(int count)
    {
        coinCount += count;
        if (coinCount < 0)
            coinCount = 0;
    }

    public bool IsNoAds()
    {
        return noAds;
    }

    public void NextLevel()
    {
        curLevelId += 1;
    }

    public void ChangeLanguage(LanguageType type)
    {
        languageType = type;
    }
}

[Serializable]
public class GameData
{
    public List<LevelDataInfor> levelDataInfors = new List<LevelDataInfor>();

    public int GetAmountLevels()
    {
        return levelDataInfors.Count;
    }
    public LevelDataInfor GetLevelDataInforById(int id)
    {
       LevelDataInfor levelDataInfor = levelDataInfors.Find(x => x.id == id);
        return levelDataInfor;
    }
    public int FindLevelToUnlock()
    {
       var levels =  levelDataInfors.OrderBy(x => x.id).ToList();
       var levelUnlock =  levels.Find(level => level.stateLv == StateLv.Lock);
        if(levelUnlock != null)
        {
            return levelUnlock.id;
        }
        return -1;
    }

    public void UnlockLevel(int id)
    {
        LevelDataInfor levelDataInfor = GetLevelDataInforById(id);
        if (levelDataInfor != null && levelDataInfor.stateLv == StateLv.Lock)
        {
            levelDataInfor.stateLv = StateLv.UnLock;
        }
    }
}
[Serializable]
public class GameSetting
{
    public bool musicState; // trạng thái nhạc
    public float musicValue;
    public bool soundState; // trạng thái âm thanh
    public float soundValue;
    public bool vibrateState; // rung
    public GameSetting()
    {
        musicState = true;
        soundState = true;
        vibrateState = true;
        musicValue = 1;
        soundValue = 1;
    }
}
[Serializable]
public class TempData
{
    public int rewardedVideoCount;
    public string lastTimeBBButonShowAd;
    public int winLevelCount;
    public int loseLevelCount;
    public int earnedGemCount;
    public bool push_earnedGemCount_event;
    public float spentIAP;
    public bool push_spentIAP_event;
    public bool push_retention_day7;
    public bool push_retention_day5;
    public bool push_retention_day3;
    public bool push_firstIAP_event;

    public TempData()
    {
        rewardedVideoCount = 0;
        lastTimeBBButonShowAd = DateTime.Now.ToString();
    }
}

[Serializable]
public class LevelDataInfor
{
    public int id;
    public StateLv stateLv;
    public int starCount; //0, 1, 2, 3 stars
    public int timeLimit;
}
public enum StateLv
{
    Lock = 1,
    UnLock = 2,
    Completed = 3,
}