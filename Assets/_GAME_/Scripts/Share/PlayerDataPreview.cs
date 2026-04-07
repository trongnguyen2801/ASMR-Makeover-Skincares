using UnityEngine;
#if UNITY_EDITOR
using Popup;
using UnityEditor;
#endif

public class PlayerDataPreview : MonoBehaviour
{
#if UNITY_EDITOR
    public PlayerData playerData;

    private void Start()
    {
        playerData = PlayerData.current;
    }

    public void Save()
    {
        Model.Instance.Save();
    }

    public void Load()
    {
        Model.Instance.Load();
        playerData = PlayerData.current;
        // = playerData.level;
    }

    public void Reload()
    {
        playerData = PlayerData.current;
    }

    public int levelIndex;

    public void HackLevel()
    {
        Model.Instance.Load();
        //PlayerData.current.level = levelIndex;
        Model.Instance.Save();
    }

    public void HackCoinGem()
    {
        PlayerData.current.AddCoin(10000);
        PlayerData.current.AddGem(500);
    }

    public PopupType popupType = PopupType.PopupSetting;
    public void TestPopup()
    {
        Popup.PopupSystem.Instance.ShowPopup(popupType, CurrentPopupBehaviour.Close, true);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerDataPreview))]
public class PlayerDataPreviewEditor : Editor
{
    PlayerDataPreview playerDataPreview;

    void OnEnable()
    {
        playerDataPreview = target as PlayerDataPreview;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            playerDataPreview.Load();
        }

        if (GUILayout.Button("Reload"))
        {
            playerDataPreview.Reload();
        }

        if (GUILayout.Button("Save"))
        {
            playerDataPreview.Save();
        }

        if (GUILayout.Button("Save Level"))
        {
            playerDataPreview.HackLevel();
        }

        if (GUILayout.Button("Hack Gem & Coin"))
        {
            playerDataPreview.HackCoinGem();
        }

        if (GUILayout.Button("Test Popup"))
        {
            playerDataPreview.TestPopup();
        }
    }
}
#endif