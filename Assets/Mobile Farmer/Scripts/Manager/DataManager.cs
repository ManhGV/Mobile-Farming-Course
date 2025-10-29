using System;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Data")]
    [SerializeField] private CropData[] _cropDatas;
    
    public Sprite GetCropSpriteFromCropType(CropType cropType)
    {
        for (int i = 0; i < _cropDatas.Length; i++)
            if (_cropDatas[i].cropType == cropType)
                return _cropDatas[i].icon;
        
        Debug.LogError("Null crop type");
        return null;
    }
}
