using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    
    public void CropHarvestedCallback(CropType cropType)
    {
        bool cropFound = false;
        InventoryItem item;
        for (int i = 0; i < items.Count; i++)
        {
            item = items[i];
            if (item.cropType == cropType)
            {
                item.amount++;
                cropFound = true;
                break;
            }
        }
        
        if(cropFound)
            return;
        items.Add(new InventoryItem(cropType, 1));
    }

    public InventoryItem[] GetInventoryItemsArray() => items.ToArray();
    
    public void DebugInventoryItem()
    {
        foreach (InventoryItem item in items)
        {
            Debug.LogWarning(item.cropType + " " + item.amount);
        }
    }

    public void Clear()
    {
        items.Clear();
    }
}