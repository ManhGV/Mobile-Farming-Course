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
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop _crop;

    private void Start()
    {
        _state = TileFieldState.Empty;
    }

    public void Sow(CropData cropData)
    {
        _state = TileFieldState.Sown;
        _crop = Instantiate(cropData.cropPrefabs, transform.position, Quaternion.identity, cropParent);
    }

    public bool IsEmpty()
    {
        return _state == TileFieldState.Empty;
    }

    public bool IsSown() => _state == TileFieldState.Sown;

    public void Water()
    {
        _state = TileFieldState.Watered;
        tileRenderer.material.color = Color.white * .3f;
        _crop.ScaleUp();
    } 
}