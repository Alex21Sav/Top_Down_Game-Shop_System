
[System.Serializable] public class PlayerDate
{
    public int Coins = 0;
}


public class GameDataManger 
{

    private static PlayerDate _playerDate = new PlayerDate();

    static GameDataManger()
    {
        LoadPlayerDate();
    }

    public static int GetCoins()
    {
        return _playerDate.Coins;
    }

    public static void AddCoins(int amount)
    {
        _playerDate.Coins += amount;
        SavePlayerDate();
    }

    public static bool CanSpendCoins(int amount)
    {
         return (_playerDate.Coins >= amount);        
    }

    public static void SpendCoins(int amount)
    {
        _playerDate.Coins -= amount;
        SavePlayerDate();
    }

    static void LoadPlayerDate()
    {
        _playerDate = BinarySerializer.Load<PlayerDate>("player-data.txt");
        UnityEngine.Debug.Log("<color = green>[PlayerDate] Loaded.</color>");
    }

    static void SavePlayerDate()
    {
        BinarySerializer.Save<PlayerDate>(_playerDate, "player-data.txt");
        UnityEngine.Debug.Log("<color = magenta>[PlayerDate] Saved.</color>");
    }
}
