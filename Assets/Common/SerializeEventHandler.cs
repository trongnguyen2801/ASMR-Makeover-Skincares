using UnityEditor;
using UnityEngine;

public class SerializeEventHandler : MonoBehaviour
{
    public bool saveData = true;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        SaveData();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }

    private void SaveData()
    {
#if !UNITY_EDITOR
        saveData = true;
#endif
        if (!saveData) return;

        if (Model.Instance.isLoaded)
        {
            Model.Instance.Save();
        }

        PlayerPrefs.Save();
    }
#if UNITY_EDITOR

    [MenuItem("Assets/Clear Data")]
    static void ClearData()
    {
        Model.Instance.ClearData();
        PlayerPrefs.DeleteAll();
    }
#endif
}