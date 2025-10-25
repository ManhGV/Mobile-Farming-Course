using System;
using UnityEngine;

public enum TileFieldState
{
    Empty,
    Sown, // Trồng cây rồi
    Watered // Tưới nước rồi
}

public class CropTile : MonoBehaviour
{
    private TileFieldState _state;

    [Header("Elements")] 
    [SerializeField] private Transform cropParent;

    private void Start()
    {
        _state = TileFieldState.Empty;
    }

    public bool IsEmpty()
    {
        return _state == TileFieldState.Empty;
    }

    public void Sow(CropData cropData)
    {
        _state = TileFieldState.Sown;
        Crop crop = Instantiate(cropData.cropPrefabs, transform.position, Quaternion.identity, cropParent);
    }
}