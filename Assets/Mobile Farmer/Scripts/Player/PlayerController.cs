using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAimator))]
public class PlayerController : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private MobileJoystick _joystick;
    private PlayerAimator _playerAnimator;
    private CharacterController _characterController;
    [SerializeField] private float _moveSpeed;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<PlayerAimator>();
    }

    private void Update()
    {
        ManagerMovement();
    }

    private void ManagerMovement()
    {
        Vector3 moveVector = _joystick.GetMovementVector() * _moveSpeed * Time.deltaTime / Screen.width;
        
        moveVector.z = moveVector.y;
        moveVector.y = 0;
        
        _characterController.Move(moveVector);

        _playerAnimator.ManageAnimations(moveVector);
    }
}