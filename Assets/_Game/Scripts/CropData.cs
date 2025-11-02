using UnityEngine;

[CreateAssetMenu(fileName = "Crop Data", menuName = "Scriptable Object/Crop Data", order = 0)]
public class CropData : ScriptableObject
{
    [Header("Settings")] 
    public Crop cropPrefabs;
    public CropType cropType;
    public Sprite icon;
    public int price;
}
