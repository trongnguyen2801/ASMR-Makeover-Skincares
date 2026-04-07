using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public Button btn;
    public Text texLvId;
    public Image lockImg;
    public Image thumb;

    public LevelDataInfor levelDataInfor;


    public void Init(LevelDataInfor levelDataInfor)
    {
        texLvId.text = "Level" + levelDataInfor.id.ToString();
        this.levelDataInfor = levelDataInfor;
        var levelDataStatic = LevelDataSO.Instance.GetDataLvStaticById(levelDataInfor.id);
        SetProperties(levelDataInfor, levelDataStatic);
    }

    private void SetProperties(LevelDataInfor levelDataInfor, LevelDataStatic levelDataStatic)
    {
        btn.onClick.RemoveAllListeners();
        switch (levelDataInfor.stateLv)
        {
            case StateLv.Lock:

                lockImg.gameObject.SetActive(true);
                btn.onClick.AddListener(() =>
                {
                    //open popupUnlock
                    //  EnterPlay();
                    PopupUtility.OpenPopupLiteMessage("Completed precious level to unlock!");
                    Debug.Log("Level is locked, cannot play");
                });
                break;
            case StateLv.UnLock:
                //ui
                lockImg.gameObject.SetActive(false);
                btn.onClick.AddListener(() =>
                {
                    EnterPlay();
                    Debug.Log("Level is Unlock, enter play");
                });
                break;
            case StateLv.Completed:
                lockImg.gameObject.SetActive(false);
                btn.onClick.AddListener(() =>
                {
                    EnterPlay();
                    Debug.Log("Level is Completed,  play again");
                });
                break;
        }

        thumb.sprite = levelDataStatic.thumb;
    }

    private void EnterPlay()
    {
        PlayerData.current.curLevelId = levelDataInfor.id;
        GameManager.Instance.transitionManager.PlayTransition(() =>
        {
            GameManager.Instance.uiManager.ActiveScreen<GamePlayScreen>();
        });
    }
}