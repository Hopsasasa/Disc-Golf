using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour, IDataPersistence
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject gameSetupMenu;
    public GameObject gameMenu;

    public void CreateGame()
    {
        bool x = GameCreator.Instance.SetupTrackSimple();
        if (!x) { return; }

        gameMenu.SetActive(true);
        gameSetupMenu.SetActive(false);
    }
    public void SaveAndExit()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void LoadData(SaveData data) { }

    public void SaveData(ref SaveData data)
    {
        long dateTime = ((System.DateTimeOffset) System.DateTime.Now).ToUnixTimeSeconds();
        int[][] track = GameCreator.Instance.GetReversedTrack();
        int[] results = GameCreator.Instance.GetResults();
        string[] playerNames = GameCreator.Instance.GetPlayerNames();
        string trackName = GameCreator.Instance.GetTrackName();

        Player[] players = new Player[track.Length];

        for (int i = 0; i < track.Length; i++)
        {
            Player player = new Player(playerNames[i], track[i]);
            players[i] = player;
        }

        GameData newGameData = new GameData(dateTime, players, results, trackName);

        if (data.firstGameDate.Equals(0))
        {
            data.firstGameDate = dateTime;
        }

        data.games.Add(newGameData);
    }
}
