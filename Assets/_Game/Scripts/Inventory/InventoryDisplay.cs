using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Transform _uiCropContainerParent;
    [SerializeField] private UICropContainer _uiCropContainerPrefabs;
    
    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItemsArray();
        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer newCropContainer = Instantiate(_uiCropContainerPrefabs, _uiCropContainerParent);
            newCropContainer.Configure(DataManager.instance.GetCropSpriteFromCropType(items[i].cropType),items[i].amount);
        }
    }

    public void UpdateDislay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItemsArray();
        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer newCropContainer;
            if (i < _uiCropContainerParent.childCount)
            {
                newCropContainer = _uiCropContainerParent.GetChild(i).GetComponent<UICropContainer>();
                newCropContainer.gameObject.SetActive(true);
            }
            else
                newCropContainer = Instantiate(_uiCropContainerPrefabs, _uiCropContainerParent);
            
            newCropContainer.Configure(DataManager.instance.GetCropSpriteFromCropType(items[i].cropType), items[i].amount);
        }

        int remaningContainers = _uiCropContainerParent.childCount - items.Length;
        
        if(remaningContainers <= 0)
            return;

        for (int i = 0; i < remaningContainers; i++)
            _uiCropContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
    }
}
