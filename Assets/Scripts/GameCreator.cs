using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCreator : MonoBehaviour
{
    public static GameCreator Instance { get; private set; }

    [Header("Fields")]
    public TMP_InputField[] playerNameFields;
    public TMP_InputField holeAmountField;
    public TMP_InputField trackNameField;

    [Header("Prefabs")]
    public GameObject pointsInputfieldPrefab;
    public GameObject holeParentPrefab;
    public GameObject playerTextPrefab;

    // Configs Menu
    public GameObject playerConfigPrefab;

    [Header("Parents")]
    public GameObject scrollableWindowParent;
    public GameObject playerNamesParent;
    public GameObject resultsParent;
    public TextMeshProUGUI trackNameText;

    // Config Menu
    public GameObject configPlayerParent;

    private int playerCount = 1;
    private string[] playerNames;
    private TMP_InputField[][] track;
    private TextMeshProUGUI[] results;
    private string trackName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public (string, TMP_InputField[][]) SetupTrack()
    {
        trackNameText.text = trackNameField.text;

        // Generating names
        playerNames = new string[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            if (playerNameFields[i] == null) { continue; }

            GameObject nameObj = Instantiate(playerTextPrefab, playerNamesParent.transform);
            nameObj.GetComponentInChildren<TextMeshProUGUI>().text = playerNameFields[i].text;
            playerNames[i] = playerNameFields[i].text;

        }

        // Generating result textfields
        results = new TextMeshProUGUI[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            if (playerNameFields[i] == null) { continue; }

            GameObject nameObj = Instantiate(playerTextPrefab, resultsParent.transform);
            TextMeshProUGUI tmpui = nameObj.GetComponentInChildren<TextMeshProUGUI>();
            tmpui.text = 0.ToString();
            results[i] = tmpui;

        }

        int holeAmount = int.Parse(holeAmountField.text);

        TMP_InputField[][] inputFields = new TMP_InputField[holeAmount][]; 

        // Generating hole fields
        for (int i = 1; i <= holeAmount; i++)
        {
            inputFields[i - 1] = new TMP_InputField[playerCount];
            Transform holeParentObj = Instantiate(holeParentPrefab, scrollableWindowParent.transform).transform;
            
            holeParentObj.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

            for (int y = 0; y < playerCount; y++)
            {
                if (playerNameFields[y] == null) { continue; }
                TMP_InputField tmp = Instantiate(pointsInputfieldPrefab, holeParentObj.GetChild(1)).GetComponent<TMP_InputField>();
                inputFields[i - 1][y] = tmp;
            }
        }
        trackName = trackNameField.text;
        track = inputFields;
        return (trackName, track);
    }

    public bool SetupTrackSimple()
    {
        if (playerNameFields[0] == null || playerNameFields[0].text.Equals("")) { return false; }
        if (holeAmountField.text.Equals("")) { return false; }
        if (trackNameField.text.Equals("")) { return false; }

        SetupTrack();

        return true;
    }

    public void AddPlayer()
    {
        if (playerNameFields.Length <= playerCount)
        {
            return;
        }
        GameObject playerObj = Instantiate(playerConfigPrefab, configPlayerParent.transform);
        playerNameFields[playerCount] = playerObj.transform.GetComponentInChildren<TMP_InputField>();
        playerCount++;
        playerObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + playerCount;
    }

    public void RemovePlayer()
    {
        if (playerCount < 1)
        {
            return;
        }
        playerCount--;
        Destroy(playerNameFields[playerCount].transform.parent.gameObject);
        playerNameFields[playerCount] = null;
    }

    public void UpdateResults()
    {
        int[] sums = new int[playerCount];
        for (int i = 0; i < track.Length; i++)
        {
            for (int y = 0; y < track[i].Length; y++)
            {
                if (track[i][y].text.Equals("")) { continue; }
                sums[y] += int.Parse(track[i][y].text);
            }
        }

        for (int i = 0; i < results.Length; i++)
        {
            results[i].text = sums[i].ToString();
        }
    }

    public void ResetGame()
    {
        for (int i = 0; i < scrollableWindowParent.transform.childCount; i++)
        {
            Destroy(scrollableWindowParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerNamesParent.transform.childCount; i++)
        {
            Destroy(playerNamesParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < resultsParent.transform.childCount; i++)
        {
            Destroy(resultsParent.transform.GetChild(i).gameObject);
        }

        trackNameText.text = "";
        trackName = "";
    }

    public void ResetConfig()
    {
        for (int i = 1; i < configPlayerParent.transform.childCount; i++)
        {
            Destroy(configPlayerParent.transform.GetChild(i).gameObject);
        }

        playerCount = 1;
        trackNameField.text = "";
        holeAmountField.text = "";
    }


    public string[] GetPlayerNames()
    {
        return playerNames;
    }

    public int[][] GetReversedTrack()
    {
        int[][] intTrack = new int[track[0].Length][];

        for (int i = 0; i < track[0].Length; i++)
        {
            intTrack[i] = new int[track.Length];
        }

        for (int i = 0; i < track.Length; i++)
        {
            for (int y = 0; y < track[i].Length; y++)
            {
                if (track[i][y].text.Equals(""))
                {
                    intTrack[y][i] = 0;
                }
                else
                {
                    intTrack[y][i] = int.Parse(track[i][y].text);
                }
            }
        }

        return intTrack;
    }

    public int[] GetResults()
    {
        int[] intResults = new int[results.Length];
        for (int i = 0; i < results.Length; i++)
        {
            intResults[i] = int.Parse(results[i].text);
        }
        return intResults;
    }

    public string GetTrackName()
    {
        return trackName;
    }

}
