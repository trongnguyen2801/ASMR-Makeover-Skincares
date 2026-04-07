using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreen : ScreenUI
{
    public Transform levelHolder;
    public LevelItem levelItemUiPrefab;

    public override void Active()
    {
        base.Active();
        InitUiItemLevel();
    }

    public void InitUiItemLevel()
    {
        for (int i = levelHolder.childCount - 1; i >= 0; i--)
        {
            try
            {
                Destroy(levelHolder.GetChild(i).gameObject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        int amoutLevels = PlayerData.current.gameData.GetAmountLevels();
        for (int i = 0; i < amoutLevels; i++)
        {
            LevelDataInfor levelDataInfor = PlayerData.current.gameData.GetLevelDataInforById(i + 1);
            if (levelDataInfor == null) continue;
            LevelItem levelItem = Instantiate(levelItemUiPrefab, levelHolder);
            levelItem.Init(levelDataInfor);
        }
    }
    public void ClearLevels()
    {
        for (int i = levelHolder.childCount - 1; i >= 0; i--)
        {
            try
            {
                Destroy(levelHolder.GetChild(i).gameObject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
