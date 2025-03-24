using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "WuluMongaloo";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public SaveData LoadGame()
    {

        string fullPath = Path.Combine(dataDirPath, dataFileName);

        SaveData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {

                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }

                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);


            }
            catch (Exception e)
            {
                Debug.LogError("Something terrible happened when trying to load saved data! " + e);
            }
        }

        return loadedData;

    }

    public void SaveGame(SaveData data)
    {

        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }

            }

        }catch (Exception e)
        {
            Debug.LogError("Something terrible happened when trying to save! " + e);
        }

    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    public void DeleteSave()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            File.Delete(fullPath);
            Debug.Log("Save Succesfully deleted!");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

}
