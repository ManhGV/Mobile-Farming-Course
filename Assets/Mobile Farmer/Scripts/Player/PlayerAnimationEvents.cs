using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private ParticleSystem seedParticle;
    [SerializeField] private ParticleSystem waterParticle;

    private void PlaySeedParticle()
    {
        seedParticle.Play();
    }

    private void PlayerWaterParticle()
    {
        waterParticle.Play();
    }
}
