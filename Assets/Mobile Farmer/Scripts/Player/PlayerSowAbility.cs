using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAimator))]
public class PlayerSowAbility : MonoBehaviour
{
    [Header("Element")] 
    private PlayerAimator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAimator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField"))
            playerAnimator.PlaySowAnimation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
            playerAnimator.StopSowAnimation();
    }
}
