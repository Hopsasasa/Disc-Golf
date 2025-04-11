using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HistoryController : MonoBehaviour
{
    public static HistoryController Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject gameIndexPrefab;
    public GameObject holeParentPrefab;
    public GameObject holeScorePrefab;
    public GameObject playerNamePrefab;
    public GameObject resultsPrefab;

    [Header("Parents")]
    public GameObject gameDetailsMenuParent;
    public Transform gameIndexParent;
    public Transform contentParent;
    public Transform playerNamesParent;
    public Button deleteGameButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Found more than one HistoryController scripts in the scene. Disabling other...");
            GetComponent<HistoryController>().enabled = false;
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DisplayGamesList()
    {

        SaveData data = DataPersistenceManager.Instance.HardLoadData();

        if (data == null || data.firstGameDate == 0) { return; }

        List<GameData> games = data.games;
        games.Reverse();

        foreach (GameData game in data.games)
        {
            GameObject obj = Instantiate(gameIndexPrefab, gameIndexParent);

            obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = System.DateTimeOffset.FromUnixTimeSeconds(game.dateTime).ToString("dd/MM/yy \n HH:mm");
            obj.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = game.trackName;
            string players = "";
            foreach (Player player in game.players)
            {
                players += player.playerName + " ";
            }
            obj.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = players;
            obj.GetComponent<GameIndexController>().data = game;
        }
    }

    public void DeleteDisplay()
    {
        for (int i = 0; i < gameIndexParent.childCount; i++)
        {
            Destroy(gameIndexParent.GetChild(i).gameObject);
        }
    }

    public void DeleteDetailedDisplay()
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerNamesParent.childCount; i++)
        {
            Destroy(playerNamesParent.GetChild(i).gameObject);
        }
    }

    public void DisplayGameDetails(GameData data, GameIndexController gic)
    {
        gameDetailsMenuParent.SetActive(true);
        GenerateDetailedGameView(data, gic);
    }

    public void GenerateDetailedGameView(GameData data, GameIndexController gic)
    {
        deleteGameButton.onClick.AddListener(gic.DeleteGame);
        for (int i = 0; i < data.players.Length; i++)
        {
            GameObject player = Instantiate(playerNamePrefab, playerNamesParent);
            player.GetComponentInChildren<TextMeshProUGUI>().text = data.players[i].playerName;
        }

        for (int i = 0; i < data.players[0].scores.Length; i++)
        {
            GameObject line = Instantiate(holeParentPrefab, contentParent);
            line.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

            for (int y = 0; y < data.players.Length; y++)
            {
                GameObject holePlayer = Instantiate(holeScorePrefab, line.transform.GetChild(1).transform);
                holePlayer.GetComponentInChildren<TextMeshProUGUI>().text = data.players[y].scores[i].ToString();
            }

        }

        GameObject results = Instantiate(resultsPrefab, contentParent);

        for (int i = 0; i < data.result.Length; i++)
        {
            GameObject result = Instantiate(holeScorePrefab, results.transform.GetChild(1).transform);
            result.GetComponentInChildren<TextMeshProUGUI>().text = data.result[i].ToString();
        }

    }

}
