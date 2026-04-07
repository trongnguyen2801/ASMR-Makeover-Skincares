using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestLvDev : MonoBehaviour
{
    public BaseLevelCtrl levelCtrlTest;
    public int timeLitmitLvTest = 120;
    public GamePlayScreen gamePlayScreen;
    public void TestLv()
    {
        GameManager.Instance.uiManager.Initialize(GameManager.Instance);
        gamePlayScreen.LoadLvTest(levelCtrlTest, timeLitmitLvTest);
        
    }
    private void Start()
    {
        DOVirtual.DelayedCall(1f, TestLv);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(TestLvDev))]
public class MapDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TestLvDev TestLvDev = (TestLvDev)target;
        if (GUILayout.Button("Test Lv"))
        {
            TestLvDev.TestLv();
        }
    }
}
#endif
