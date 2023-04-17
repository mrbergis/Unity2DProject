using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    Down,
    Up,
    Left,
    Right
}
public class PlayerLogic : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    float _movementSpeed = 5.0f;
    float _horizontalInput;
    float _verticalInput;
    Vector2 _movementInput;
    Vector2 _movementVelocity;
    
    MoveDirection _movementDirection = MoveDirection.Down;
    
    Animator _animator;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        UpdateMovementInput();
        UpdateMovementDirection();
        
        if(Input.GetButtonDown("Fire1"))
        {
            _animator.SetBool("IsAttacking", true);
        }
    }
    
    void UpdateMovementInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        _movementInput = new Vector2(_horizontalInput, _verticalInput);
        _movementVelocity = _movementInput.normalized * _movementSpeed;
        
        if(_animator)
        {
            _animator.SetFloat("MovementInput", _movementInput.magnitude);
        }
    }

    void UpdateMovementDirection()
    {
        if (Mathf.Abs(_horizontalInput) > Mathf.Abs(_verticalInput))
        {
            if (_horizontalInput > 0)
            {
                _movementDirection = MoveDirection.Right;
            }
            else if (_horizontalInput < 0)
            {
                _movementDirection = MoveDirection.Left;
            }
        }
        else if (Mathf.Abs(_horizontalInput) < Mathf.Abs(_verticalInput))
        {
            if (_verticalInput > 0)
            {
                _movementDirection = MoveDirection.Up;
            }
            else if (_verticalInput < 0)
            {
                _movementDirection = MoveDirection.Down;
            }
        }

        if (_animator)
        {
            _animator.SetInteger("MovementDirection", (int)_movementDirection);
        }
    }
    
    private void FixedUpdate()
    {
        if (_rigidbody)
        {
            _rigidbody.MovePosition(_rigidbody.position + _movementVelocity * Time.deltaTime);
        }
    }
}
