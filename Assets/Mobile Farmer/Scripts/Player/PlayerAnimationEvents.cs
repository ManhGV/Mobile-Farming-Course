using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private ParticleSystem seedParticle;

    private void PlaySeedParticle()
    {
        seedParticle.Play();
    }
}
