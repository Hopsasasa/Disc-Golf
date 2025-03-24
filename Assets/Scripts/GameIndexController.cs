using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIndexController : MonoBehaviour
{
    public GameData data;

    public void OpenDetails()
    {
        HistoryController.Instance.DisplayGameDetails(data); 
    } 
}
