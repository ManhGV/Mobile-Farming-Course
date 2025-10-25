using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAimator))]
public class PlayerSowAbility : MonoBehaviour
{
    [Header("Element")] 
    private PlayerAimator playerAnimator;
    
    [Header("Settings")]
    private CropField currentCropField;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAimator>();

        SeedParticles.onSeedsCollided += SeedsCollidedCallback;
        CropField.OnFullySown += CropFieldFullySownCallback;
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
        if (other.CompareTag("CropField")&&other.GetComponent<CropField>().IsEmpty())
        {
            playerAnimator.PlaySowAnimation();
            currentCropField = other.GetComponent<CropField>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }
}
