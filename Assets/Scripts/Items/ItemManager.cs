using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ebac.Core.Singleton;

public class ItemManager : Singleton<ItemManager>
{

    public int coins;
    public TextMeshProUGUI CoinCounterText;

    private void Start()
    {
        Reset();
        UpdateCoinCounter();
    }

    private void Reset()
    {
        coins = 0;
    }

    public void AddCoins(int amount = 1)
    {
        coins += amount;
        UpdateCoinCounter();
    }
    private void UpdateCoinCounter()
    {
        if (CoinCounterText != null)
        {
            CoinCounterText.text = "Moedas: " + coins.ToString();
        }
    }
}
