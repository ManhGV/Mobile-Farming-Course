using UnityEngine;

public class PlayerAimator : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem waterParticles;

    [Header("Settings")] 
    [SerializeField] private float moveSpeedMultiper;
    public void ManageAnimations(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            _animator.SetFloat("moveSpeed",moveVector.magnitude*moveSpeedMultiper);
            PlayRunAnimation();
            _animator.transform.forward = moveVector.normalized;
        }
        else
            PlayIdleAnimator();
    }

    private void PlayIdleAnimator()
    {
        _animator.Play("Idle");
    }

    private void PlayRunAnimation()
    {
        _animator.Play("Run");
    }
    
    public void PlaySowAnimation()
    {
        _animator.SetLayerWeight(1, 1);
    }

    public void StopSowAnimation()
    {
        _animator.SetLayerWeight(1, 0);
    }

    public void PlayWaterAnimation()
    {
        _animator.SetLayerWeight(2, 1);
    }

    public void StopWaterAnimation()
    {
        _animator.SetLayerWeight(2, 0);
        waterParticles.Stop();
    }

    public void PlayHavertAnimation()
    {
        _animator.SetLayerWeight(3, 1);
    }
    public void StopHavertAnimation()
    {
        _animator.SetLayerWeight(3, 0);
    }
}
