using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDataStatic
{
    public int id; //start from 1
    public BaseLevelCtrl prefab; //Prefab for the level
    public int timeLimit;
    public Sprite thumb;
}

