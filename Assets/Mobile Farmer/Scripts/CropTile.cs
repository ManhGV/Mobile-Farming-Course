using System;
using UnityEngine;
using UnityEngine.Events;

public class CropTile : MonoBehaviour
{
    private TileFieldState _state;

    [Header("Elements")] 
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop _crop;
    private CropData _cropData;

    [Header("Events")]
    public static Action<CropType> OnCropHavested;

    private void Start()
    {
        _state = TileFieldState.Empty;
    }

    public void Sow(CropData cropData)
    {
        _state = TileFieldState.Sown;
        _crop = Instantiate(cropData.cropPrefabs, transform.position, Quaternion.identity, cropParent);
        
        _cropData = cropData;
    }

    public bool IsEmpty()
    {
        return _state == TileFieldState.Empty;
    }

    public bool IsSown() => _state == TileFieldState.Sown;

    public void Water()
    {
        _state = TileFieldState.Watered;
        tileRenderer.gameObject.LeanColor(Color.white * .3f, 1);
        _crop.ScaleUp();
    }

    public void Harvest()
    {
        _state = TileFieldState.Empty;
        _crop.ScaleDown();
        tileRenderer.gameObject.LeanColor(Color.white, 1);
        
        OnCropHavested?.Invoke(_cropData.cropType);
    }
}