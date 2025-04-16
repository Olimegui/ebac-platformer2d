using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ebac.Core.Singleton;

public class ItemManager : Singleton<ItemManager>
{

    public SOInt coins;
    public TextMeshProUGUI CoinCounterText;

    private void Start()
    {
        Reset();
        UpdateCoinCounter();
    }

    private void Reset()
    {
        coins.value = 0;
    }

    public void AddCoins(int amount = 1)
    {
        coins.value += amount;
        UpdateCoinCounter();
    }
    
    private void UpdateCoinCounter()
    {
        if(CoinCounterText != null)
        {
          CoinCounterText.text = "Moedas" + coins.value.ToString();
        }
    }
}
