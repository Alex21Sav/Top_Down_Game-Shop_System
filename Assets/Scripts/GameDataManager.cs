using System.Collections.Generic;
[System.Serializable] public class CharacterShopDate
{
    public List<int> purchasedCharacterIndex = new List<int>();
}
[System.Serializable] public class PlayerDate
{
    public int Coins = 0;
    public int selectedCharacterIndex = 0;
}

public class GameDataManager 
{

    private static PlayerDate _playerDate = new PlayerDate();
    private static CharacterShopDate _characterShopDate = new CharacterShopDate();

    private static Character _selectedCharacter;

    static GameDataManager()
    {
        LoadPlayerDate();
        LoadCharacterShopDate();
    }
    
    public static Character GetSelectedCharacter()
    {
        return _selectedCharacter;
    }

    public static void SetSelectedCharacter(Character character, int index)
    {
        _selectedCharacter = character;
        _playerDate.selectedCharacterIndex = index;
        SavePlayerDate();

    }

    public static int GetSelectedCharacterIndex()
    {
        return _playerDate.selectedCharacterIndex;
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
        BinarySerializer.Save(_playerDate, "player-data.txt");
        UnityEngine.Debug.Log("<color = magenta>[PlayerDate] Saved.</color>");
    }

    public static void AddCharacterPurchase(int characterIndex)
    {
        _characterShopDate.purchasedCharacterIndex.Add(characterIndex);
        SaveCharacterShopDate();
    }

    public static List<int> GetAllCharacterPurchase()
    {
        return _characterShopDate.purchasedCharacterIndex;  
        
    }
    public static int GetCharacterPurchase( int index)
    {
        return _characterShopDate.purchasedCharacterIndex[index];
    }

    static void LoadCharacterShopDate()
    {
        _characterShopDate = BinarySerializer.Load<CharacterShopDate>("character-shop-date.txt");
        UnityEngine.Debug.Log("<color = green>[CharacterShopDate] Loaded.</color>");
    }

    static void SaveCharacterShopDate()
    {
        BinarySerializer.Save(_characterShopDate, "character-shop-date.txt");
        UnityEngine.Debug.Log("<color = magenta>[CharacterShopDate] Saved.</color>");
    }
}
