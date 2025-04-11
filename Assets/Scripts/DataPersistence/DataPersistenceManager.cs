using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Configuration")]
    [SerializeField] private string fileName = "save.digo";
    [SerializeField] public bool useEncryption = false;

    private SaveData SaveData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager Instance { get; private set; }
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Found more than one DataPersistenceManager scripts in the scene. Disabling other...");
            this.GetComponent<DataPersistenceManager>().enabled = false;
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }

    private void Start()
    {
        dataHandler = GetDataHandler();
    }

    public void NewGame()
    {
        SaveData = new SaveData();
        dataHandler.SaveGame(SaveData);
    }

    public void DeleteSave()
    {
        dataHandler = GetDataHandler();
        dataHandler.DeleteSave();
    }

    public void LoadGame()
    {
        SaveData = dataHandler.LoadGame();

        if (SaveData == null)
        {
            Debug.Log("No saved data found. Initializing to defaults!");
            NewGame();
        }

        List<IDataPersistence> tempObjects = new List<IDataPersistence>(dataPersistenceObjects);
        
        foreach (IDataPersistence dataPersistenceObj in tempObjects)
        {
            dataPersistenceObj.LoadData(SaveData);
        }
    }

    public SaveData HardLoadData()
    {
        SaveData = dataHandler.LoadGame();
        return SaveData;
    }

    public void HardSaveData(SaveData data)
    {
        SaveData = data;
        dataHandler.SaveGame(SaveData);
    }

    public void DeleteGame(GameData gameData)
    {
        SaveData = HardLoadData();
        int index = SaveData.games.FindIndex(data => data.dateTime == gameData.dateTime);
        SaveData.games.RemoveAt(index);
        HardSaveData(SaveData);

    }

    public void SaveGame()
    {
        if (HardLoadData() == null)
        {
            SaveData = new SaveData();
        }
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref SaveData);
        }

        dataHandler.SaveGame(SaveData);
        Debug.Log("Saved Game");

    }

    private FileDataHandler GetDataHandler()
    {
        if (dataHandler == null)
        {
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        }
        return dataHandler;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {

        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);

    }
}
