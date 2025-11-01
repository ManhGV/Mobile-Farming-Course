using System;
using TMPro;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private BoxCollider boxTrigger;

    [Header("Setting")]
    [SerializeField] private int initialPrice; 
    private int currentPrice;
    private bool unlocked = false;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;
    private void Start()
    {
        currentPrice = initialPrice;
        priceText.text = currentPrice.ToString();
    }

    public void Initialize(int wordDataChunkPrice)
    {
        
        currentPrice = wordDataChunkPrice;
        priceText.text = currentPrice.ToString();
        
        if(wordDataChunkPrice <= 0)
            Unlock(false);
    }
    
    public void TryUnlock()
    {
        if(CashManager.instance.GetCoint()<=0)
            return;
        currentPrice--;
        CashManager.instance.UseCoins(1);
        onUnlocked?.Invoke();
        
        priceText.text = currentPrice.ToString();
        if (currentPrice <= 0)
            Unlock();
    }

    private void Unlock(bool CanSave = true)
    {
        unlockedElements.SetActive(true);
        lockedElements.SetActive(false);
        boxTrigger.enabled = false;
        unlocked = true;
        if(CanSave) 
            onUnlocked?.Invoke();
    }

    public bool IsUnlocked() => unlocked;
    
    public int GetCurrentPrice() => currentPrice;
    
    public int GetInitialPrice() => initialPrice;
}
