using DG.Tweening;
using Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreen : ScreenUI
{
    public GameObject BoardGame;
    public BaseLevelCtrl curLevelCtrl;
    public LevelStateCtrl curIntancelv => curLevelCtrl.CurInstanceStateLv;
    public Text timeCount;
    public float timeLimit;
    public bool isPause = false;
    [SerializeField] private Animator iconStep;


    public override void Initialize(UIManager UI)
    {
        base.Initialize(UI);
        // BBEventDispatcher.Register(BBEventId.OnWinLv, _ => { Win(); });
        //BBEventDispatcher.Register(BBEventId.OnLoseLv, _ => { Lose(); });
        //BBEventDispatcher.Register(BBEventId.OnTimeUp, _ => { TimeUp(); });
    }
    public override void Active()
    {
        base.Active();
        if (BoardGame != null)
        {
            BoardGame.SetActive(true);
        }
        GameManager.Instance.SwitchGameState(GameState.Playing);
        isPause = false;    
        LoadLv();
    }
    public override void DeActive()
    {
        base.DeActive();
        if (BoardGame != null)
        {
            BoardGame.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.GameState == GameState.Playing)
        {
           if (curIntancelv != null && curIntancelv.isFinishLv) return;
            
            if (timeLimit > 0)
            {
                timeLimit -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeLimit / 60);
                int seconds = Mathf.FloorToInt(timeLimit % 60);
                timeCount.text = $"{minutes:00}:{seconds:00}";
                if (timeLimit <= 0 && !isPause)
                {
                    isPause = true;
                    BtnPause();
                }
            }
        }
    }
    protected void LoadLv()
    {
        if (curLevelCtrl != null)
        {
           Destroy(curLevelCtrl.gameObject);
            curLevelCtrl = null;
        }
        int curIdLv = PlayerData.current.curLevelId;
        LevelDataStatic levelDataStatic = LevelDataSO.Instance.GetDataLvStaticById(curIdLv);

        if (levelDataStatic != null)
        {
            curLevelCtrl = Instantiate(levelDataStatic.prefab,BoardGame.transform);
        }
        else
        {
            Debug.LogError("Level data not found for ID: " + curIdLv);
        }

        LevelDataInfor levelDataInfor = PlayerData.current.gameData.levelDataInfors
            .Find(x => x.id == curIdLv);
        if (levelDataInfor != null)
        {
            timeLimit = levelDataInfor.timeLimit;
        }
         curLevelCtrl.Init();
    }

    public void LoadLvTest(BaseLevelCtrl levelCtrlTest,int timeLimit)
    {
        if (curLevelCtrl != null)
        {
            Destroy(curLevelCtrl.gameObject);
            curLevelCtrl = null;
        }
        curLevelCtrl = levelCtrlTest;
        this.timeLimit = timeLimit;
        GameManager.Instance.SwitchGameState(GameState.Playing);
        isPause = false;
        curLevelCtrl.Init();
    }
    public void BtnPause()
    {
        Popup.PopupSystem.GetOpenBuilder().SetType(PopupType.PopupSetting)
        .SetCurrentPopupBehaviour(CurrentPopupBehaviour.Close).SetDelayTime(0f).Open();
      //  AudioManager.Instance.PlaySFX(AudioClipId.ClickBtn);
    }
    public void ActiveIconWrong()
    {
        iconStep?.gameObject.SetActive(true);
        iconStep?.Play("wrongAtStep", -1, 0);
    }
    public void ActiveIconCorrect()
    {
        iconStep?.gameObject.SetActive(true);
        iconStep?.Play("correctAtStep", -1, 0);
    }
    
    public void ActiveIconCorrectWithPos(Vector3 point)
    {
        if (iconStep == null) return;
        iconStep.gameObject.transform.position = point;
        iconStep.gameObject.SetActive(true);
        iconStep.Play("correctAtStep", -1, 0);
    }
    public void Win()
    {
        Popup.PopupSystem.GetOpenBuilder().SetType(PopupType.PopupGameWin)
        .SetCurrentPopupBehaviour(CurrentPopupBehaviour.Close).SetDelayTime(0f).Open();
    }
}
