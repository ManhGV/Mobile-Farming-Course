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
    private int tilesWatered;
    private int tilesHarvest;

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
        EventManager.OnFullySown?.Invoke(this);
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


    public void WaterColliderCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosestCropTile(waterPositions[i]);
                
            if(closestCropTile == null)
                continue;
            
            if(!closestCropTile.IsSown())
                continue;
            
            Water(closestCropTile);
        }
    }

    private void Water(CropTile closestCropTile)
    {
        closestCropTile.Water();
        tilesWatered++;
        if (tilesWatered == _cropTiles.Count)
            FieldFullyWatered();
    }

    private void FieldFullyWatered()
    {
        state = TileFieldState.Watered;
        
        EventManager.OnFullyWatered?.Invoke(this);
    }

    public void Harvest(Transform harvestSphere)
    {
        float sphereRadius = harvestSphere.localScale.x;
        for (int i = 0; i < _cropTiles.Count; i++)
        {
            if(_cropTiles[i].IsEmpty())
                continue;
            
            float distanceCropSphere = Vector3.Distance(harvestSphere.position, _cropTiles[i].transform.position);
            if (distanceCropSphere < sphereRadius)
                HarvestTile(_cropTiles[i]);
        }
    }

    private void HarvestTile(CropTile cropTile)
    {
        cropTile.Harvest();

        tilesHarvest++;
        if (tilesHarvest == _cropTiles.Count)
            FieldFullHarvested();
    }

    private void FieldFullHarvested()
    {
        tilesSown = 0;
        tilesWatered = 0;
        tilesHarvest = 0;
        
        state = TileFieldState.Empty;
        EventManager.OnFullyHavert?.Invoke(this);
    }

    public bool IsEmpty() => state == TileFieldState.Empty;
    public bool IsSown() => state == TileFieldState.Sown;
    public bool IsWatered() => state == TileFieldState.Watered;

    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        for (int i = 0; i < _cropTiles.Count; i++)
            Sow(_cropTiles[i]);
    }

    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        for (int i = 0; i < _cropTiles.Count; i++)
            Water(_cropTiles[i]);
    }
}
