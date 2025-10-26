using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private ParticleSystem seedParticle;
    [SerializeField] private ParticleSystem waterParticle;

    [Header("Events")] 
    [SerializeField] private UnityEvent startHarvestingEvent;
    [SerializeField] private UnityEvent stopHarvestingEvent;

    private void PlaySeedParticle()
    {
        seedParticle.Play();
    }

    private void PlayerWaterParticle()
    {
        waterParticle.Play();
    }

    private void StartHarvestingCallback()
    {
        startHarvestingEvent?.Invoke();
    }

    private void StopHarvestingCallback()
    {
        stopHarvestingEvent?.Invoke();
    }
}
