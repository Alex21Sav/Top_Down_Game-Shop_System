using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterShopDatabase", menuName = "Shopping/Characters shop database")]
public class CharacterShopDatabase : ScriptableObject
{
    public Character[] Characters;

    public int CharactersCount
    {
        get { return Characters.Length;  }
    }

    public Character GetCharacter(int index)
    {
        return Characters[index];
    }

    public void PurchaseCharacter(int index)
    {
        Characters[index].IsPurchase = true;
    }
}
