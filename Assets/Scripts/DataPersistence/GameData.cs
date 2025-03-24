using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] public long dateTime;
    [SerializeField] public int[] result;
    [SerializeField] public Player[] players;
    [SerializeField] public string trackName;

    public GameData(long dateTime, Player[] players, int[] result, string trackName)
    {
        this.dateTime = dateTime;
        this.result = result;
        this.players = players;
        this.trackName = trackName;
    }

}

[System.Serializable]
public class Player
{
    [SerializeField] public string playerName;
    [SerializeField] public int[] scores;

    public Player(string playerName, int[] scores)
    {
        this.playerName = playerName;
        this.scores = scores;
    }
}
