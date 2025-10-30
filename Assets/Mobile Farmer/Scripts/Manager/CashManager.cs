using System;
using System.Resources;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;


    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI coinsEarned;
    [SerializeField] private int coins;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        LoadData();
    }
    
    public void AddCoins(int amount)
    {
        coins += amount;
        SaveData();
    }

    public void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins");
        coinsEarned.text = coins.ToString();
    }
    
    public void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
        coinsEarned.text = coins.ToString();
    }
}
