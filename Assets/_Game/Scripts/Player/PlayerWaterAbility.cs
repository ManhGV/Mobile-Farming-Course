using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAimator))]
[RequireComponent(typeof(PlayerToolSeletor))]
public class PlayerWaterAbility : MonoBehaviour
{
    [Header("Element")] 
    private PlayerAimator playerAnimator;
    private PlayerToolSeletor playerToolSeletor;
    
    [Header("Settings")]
    private CropField currentCropField;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAimator>();
        playerToolSeletor = GetComponent<PlayerToolSeletor>();

        EventManager.OnWatersCollided += WatersCollidedCallback;
        EventManager.OnFullyWatered += CropFieldFullyWateredCallback;
        EventManager.OnToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        EventManager.OnWatersCollided -= WatersCollidedCallback;
        EventManager.OnFullyWatered -= CropFieldFullyWateredCallback;
        EventManager.OnToolSelected -= ToolSelectedCallback;
    }

    private void ToolSelectedCallback(Tool selectedTool)
    {
        if(!playerToolSeletor.CanWater())
            playerAnimator.StopWaterAnimation();
    }

    private void CropFieldFullyWateredCallback(CropField obj)
    {
        if(obj == currentCropField)
            playerAnimator.StopWaterAnimation();
    }

    private void WatersCollidedCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null)
            return;
        
        currentCropField.WaterColliderCallback(waterPositions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopWaterAnimation();
            currentCropField = null;
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSeletor.CanWater())
        {
            if (currentCropField == null)
                currentCropField = cropField;
            playerAnimator.PlayWaterAnimation();
        }
    }
}
