using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelDataSO", menuName = "LevelSO", order = 0)]
public class LevelDataSO : ScriptableObject
{
    protected static LevelDataSO instance;
    public List<LevelDataStatic> levelDataStatics;
    public int AmountLevels => levelDataStatics?.Count ?? 0;
    public static LevelDataSO Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            instance = Resources.LoadAll<LevelDataSO>("").FirstOrDefault();
            return instance;
        }
    }


    public LevelDataStatic GetDataLvStaticById(int id)
    {
        var levelData = levelDataStatics.FirstOrDefault(x => x.id == id);
        if (levelData == null)
        {
            Debug.LogError($"LevelData with ID {id} not found.");
        }
        return levelData;
    }
}
