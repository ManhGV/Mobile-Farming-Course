using System;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private InventoryManager _inventoryManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
            SellCrops();
    }

    private void SellCrops()
    {
        Inventory inventory = _inventoryManager.GetInventory();
        InventoryItem[] items = inventory.GetInventoryItemsArray();

        int coinsEarned = 0;
        for (int i = 0; i < items.Length; i++)
        {
            coinsEarned += DataManager.instance.GetCropPriceFromCropType(items[i].cropType) * items[i].amount;
        }
        CashManager.instance.AddCoins(coinsEarned);
        _inventoryManager.ClearInventory();
    }
}
