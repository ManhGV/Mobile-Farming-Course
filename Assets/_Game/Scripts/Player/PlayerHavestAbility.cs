using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAimator))]
[RequireComponent(typeof(PlayerToolSeletor))]
public class PlayerHavestAbility : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private Transform harvestSphere;
    private PlayerAimator playerAnimator;
    private PlayerToolSeletor playerToolSeletor;
    
    [Header("Settings")]
    private CropField currentCropField;
    private bool canHarvest;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAimator>();
        playerToolSeletor = GetComponent<PlayerToolSeletor>();

        EventManager.OnFullyHavert += CropFieldFullyHavertCallback;
        EventManager.OnToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        EventManager.OnFullyHavert -= CropFieldFullyHavertCallback;
        EventManager.OnToolSelected -= ToolSelectedCallback;
    }

    private void ToolSelectedCallback(Tool selectedTool)
    {
        if(!playerToolSeletor.CanHarvest())
            playerAnimator.StopHavertAnimation();
    }

    private void CropFieldFullyHavertCallback(CropField obj)
    {
        if(obj == currentCropField)
            playerAnimator.StopHavertAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopHavertAnimation();
            currentCropField = null;
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSeletor.CanHarvest())
        {
            if (currentCropField == null)
                currentCropField = cropField;
            playerAnimator.PlayHavertAnimation();
            
            if(canHarvest)
                currentCropField.Harvest(harvestSphere);
        }
    }
    
    

    public void HarvestingStartedCallback()
    {
        canHarvest = true;
    }

    public void HarvestingStoppedCallback()
    {
        canHarvest = false;
    }
}
