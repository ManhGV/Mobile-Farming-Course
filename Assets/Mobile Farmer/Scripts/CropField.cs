using System;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private Transform tileParent;
    private List<CropTile> _cropTiles = new List<CropTile>();

    [Header("Settings")] 
    [SerializeField] private CropData cropData;
    private TileFieldState state;
    private int tilesSown;
    
    [Header("Actions")]
    public static Action<CropField> OnFullySown;

    void Start()
    {
        StoreTiles();
        state = TileFieldState.Empty;
    }

    private void StoreTiles()
    {
        for (int i = 0; i < tileParent.childCount; i++)
            _cropTiles.Add(tileParent.GetChild(i).GetComponent<CropTile>());
    }

    public void SeedColliderCallback(Vector3[] seedPositions)
    {
        for (int i = 0; i < seedPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(seedPositions[i]);
                
            if(closestCropTile == null)
                continue;
            
            if(!closestCropTile.IsEmpty())
                continue;
            
            Sow(closestCropTile);
        }
    }

    private void Sow(CropTile closestCropTile)
    {
        closestCropTile.Sow(cropData);
        tilesSown++;
        if (tilesSown == _cropTiles.Count)
            FieldFullySown();
    }

    private void FieldFullySown()
    {
        state = TileFieldState.Sown;
        OnFullySown?.Invoke(this);
    }

    private CropTile GetClosestCropTile(Vector3 seedPosition)
    {
        float minDistance = Single.MaxValue;
        int closestCropTileIndex = -1;
        for (int i = 0; i < _cropTiles.Count; i++)
        {
            CropTile cropTile = _cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position,seedPosition);

            if (distanceTileSeed < minDistance)
            {
                minDistance = distanceTileSeed;
                closestCropTileIndex = i;
            }
        }

        if (closestCropTileIndex == -1)
            return null;
        
        return _cropTiles[closestCropTileIndex];
    }

    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }
}
