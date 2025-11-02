using System;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    private Inventory _inventory;
    private InventoryDisplay _inventoryDisplay;
    private string dataPath;

    private void Start()
    {
        dataPath = Application.dataPath+"/inventory.txt";
        
        LoadInventory();
        
        ConfigureInventoryDisplay();
        CropTile.OnCropHavested += CropHarvestedCallback;
    }

    private void OnDestroy()
    {
        CropTile.OnCropHavested -= CropHarvestedCallback;
    }

    private void ConfigureInventoryDisplay()
    {
        _inventoryDisplay = GetComponent<InventoryDisplay>();
        _inventoryDisplay.Configure(_inventory);
    }

    private void CropHarvestedCallback(CropType cropType)
    {
        _inventory.CropHarvestedCallback(cropType);
        SaveInventory();
        _inventoryDisplay.UpdateDislay(_inventory);
    }

    [NaughtyAttributes.Button]
    public void ClearInventory()
    {
        _inventory.Clear();
        _inventoryDisplay.UpdateDislay(_inventory);
        SaveInventory();
    }

    public Inventory GetInventory() => _inventory;
    
    private void LoadInventory()
    {
        string dataPath = this.dataPath;
        string data;
        if (File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            _inventory = JsonUtility.FromJson<Inventory>(data);
            if (_inventory == null)
                _inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
            _inventory = new Inventory();
        }
        
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(_inventory, true);
        File.WriteAllText(this.dataPath, data);
    }
}
