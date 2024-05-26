using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    private static CoinManager instance;

    public static CoinManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CoinManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("CoinManager");
                    instance = obj.AddComponent<CoinManager>();
                }
            }
            return instance;
        }
    }

    private int coinCount = 0;
    private int specialCoinCount = 0;
    public Action OnCoinCountChanged;
    public Action OnSpecialCoinCountChanged;

    public int CoinCount
    {
        get { return coinCount; }
    }

    public int SpecialCoinCount
    {
        get { return specialCoinCount; }
    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log("Coin count is now: " + coinCount); // Log the new coin count
        OnCoinCountChanged?.Invoke();
    }

    public void AddSpecialCoin()
    {
        specialCoinCount++;
        Debug.Log("Special coin count is now: " + specialCoinCount); // Log the new special coin count
        OnSpecialCoinCountChanged?.Invoke();
    }
}
