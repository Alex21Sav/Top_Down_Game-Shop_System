using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameSharedUI : MonoBehaviour
{
    #region Singletone

    public static GameSharedUI Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField] private TMP_Text[] _coinsUIText;

    private void Start()
    {
        UpdateCoinsUI();
    }

    public void UpdateCoinsUI()
    {
        for (int i = 0; i < _coinsUIText.Length; i++)
        {
            SetCoinsText(_coinsUIText[i], GameDataManager.GetCoins());
        }
    }

    private void SetCoinsText(TMP_Text textMesh, int value)
    {
        if (value >= 1000)
        {
            textMesh.text = string.Format("{0}K.{1}", (value / 1000), GetFerstDigitFromNumber(value / 1000));
        }
        else
            textMesh.text = value.ToString();
    }

    private int GetFerstDigitFromNumber(int num)
    {
        return int.Parse(num.ToString()[0].ToString());
    }
}
