using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAimator))]
[RequireComponent(typeof(PlayerToolSeletor))]
public class PlayerSowAbility : MonoBehaviour
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

        SeedParticles.onSeedsCollided += SeedsCollidedCallback;
        CropField.OnFullySown += CropFieldFullySownCallback;
        playerToolSeletor.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        SeedParticles.onSeedsCollided -= SeedsCollidedCallback;
        CropField.OnFullySown -= CropFieldFullySownCallback;
        playerToolSeletor.onToolSelected -= ToolSelectedCallback;
    }

    private void ToolSelectedCallback(PlayerToolSeletor.Tool selectedTool)
    {
        if(!playerToolSeletor.CanSow())
            playerAnimator.StopSowAnimation();
    }

    private void CropFieldFullySownCallback(CropField obj)
    {
        if(obj == currentCropField)
            playerAnimator.StopSowAnimation();
    }

    private void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        if (currentCropField == null)
            return;
        
        currentCropField.SeedColliderCallback(seedPositions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSeletor.CanSow()) 
            playerAnimator.PlaySowAnimation();
    }
}
