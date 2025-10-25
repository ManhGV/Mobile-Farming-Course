using UnityEngine;

public class PlayerAimator : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Animator _animator;

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
}
