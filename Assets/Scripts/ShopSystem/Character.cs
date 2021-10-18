using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character
{
    public Sprite Image;
    public string Name;
    [Range(0, 100)] public float Speed;
    [Range(0, 100)] public float Power;
    public int Price;

    public bool IsPurchase;
}
