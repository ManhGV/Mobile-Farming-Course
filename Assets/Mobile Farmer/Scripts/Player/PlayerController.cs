using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private MobileJoystick _joystick;
    private CharacterController _characterController;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ManagerMovement();
    }

    private void ManagerMovement()
    {
        Vector3 moveVector = _joystick.GetMovementVector() * moveSpeed * Time.deltaTime / Screen.width;
        
        moveVector.z = moveVector.y;
        moveVector.y = 0;
        
        _characterController.Move(moveVector);
    }
}