using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    Pause,
    Finish,
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static GameState GameState { get; private set; }
    public UIManager uiManager;
    public TransitionManager transitionManager;
    // public TransitionManager transitionManager;
    public bool isTest;

    public override void Awake()
    {
        base.Awake();
        Model.Instance.Load();
        // AudioManager.Instance.PlayMusic( AudioClipId.BGMusic, true);
        uiManager.Initialize(this);
        //HeartManager.Instance.Init(PlayerData.current);
    }

    public void Start()
    {
        SwitchGameState(GameState.Ready);
        //if (PlayerData.current.isTheFistPlayGame)
        //{
        //    uiManager.ActiveScreen<GamePlayScreen>();
        //    PlayerData.current.isTheFistPlayGame = false;
        //}
        //else
        //    uiManager.ActiveScreen<HomeScreen>();

        if(!isTest)
            uiManager.ActiveScreen<HomeScreen>();   
    }

    public void SwitchGameState(GameState newState)
    {
        Debug.Log("ChangeGameSate :" + newState);
        GameState = newState;
        if (uiManager.ActiveScreenUI() != null)
        {
            uiManager.ActiveScreenUI().OnGameStateChanged(newState);
        }

        //if (GameState == GameState.Playing)
        //{
        //    AdvertisementManager.Instance?.ShowBannerAd();
        //}
    }

    public void SwitchGameStateNoAds(GameState newState)
    {
        Debug.Log("ChangeGameSate :" + newState);
        GameState = newState;
    }

    public void Finish()
    {
    }

    //public int GetTimeLimit(int room, int level)
    //{
    //    GameRemoteConfigHandler.TimeLimitData defConfig = null;
    //    foreach (var data in GameRemoteConfigHandler.cf_timeLimit.data)
    //    {
    //        if (data == null) continue;
    //        if (data.room == -1 && data.level == -1)
    //        {
    //            defConfig = data;
    //            continue;
    //        }

    //        if (data.room == room && data.level == level)
    //        {
    //            return data.time;
    //        }
    //    }

    //    return defConfig?.time ?? PlayerData.current.gameData.GetLevel((LevelType)room, level).timeLimit;
    //}
}