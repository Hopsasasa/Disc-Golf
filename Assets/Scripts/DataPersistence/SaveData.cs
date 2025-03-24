using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField] public long firstGameDate;
    [SerializeField] public List<GameData> games;

    public SaveData()
    {
        games = new List<GameData>();
    }

}
