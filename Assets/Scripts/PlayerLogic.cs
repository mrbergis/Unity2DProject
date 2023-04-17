using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    float _movementSpeed = 5.0f;
    float _horizontalInput;
    float _verticalInput;
    Vector2 _movementInput;
    Vector2 _movementVelocity;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        UpdateMovementInput();
    }
    
    void UpdateMovementInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        _movementInput = new Vector2(_horizontalInput, _verticalInput);
        _movementVelocity = _movementInput.normalized * _movementSpeed;
    }

    private void FixedUpdate()
    {
        if (_rigidbody)
        {
            _rigidbody.MovePosition(_rigidbody.position + _movementVelocity * Time.deltaTime);
        }
    }
}
