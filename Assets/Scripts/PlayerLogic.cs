using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection
{
    Down,
    Up,
    Left,
    Right,
    MAX
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
    
    bool _isAttacking = false;
    
    [SerializeField]
    List<BoxCollider2D> hitColliders = new List<BoxCollider2D>();
    
    const int MAX_HEALTH = 5;
    int _health = MAX_HEALTH;
    
    const float MAX_INVINCIBILITY_TIME = 0.5f;
    float _invincibilityTime = 0.0f;
    
    AudioSource _audioSource;

    [SerializeField]
    AudioClip gotHitSound;

    [SerializeField]
    AudioClip swordSlashSound;

    [SerializeField]
    AudioClip deathSound;

    [SerializeField]
    AudioClip victorySound;
    
    bool _isDead = false;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        
        UIManager.Instance.SetHealth(_health);
    }
    
    void Update()
    {
        if(_isDead)
        {
            return;
        }
        UpdateMovementInput();
        UpdateMovementDirection();
        
        if(Input.GetButtonDown("Fire1") && !_isAttacking)
        {
            SetIsAttacking(true);
            PlaySound(swordSlashSound);
        }
        if(_invincibilityTime > 0.0f)
        {
            _invincibilityTime -= Time.deltaTime;
        }
    }
    
    void UpdateAttackAnimation()
    {
        if (_animator)
        {
            _animator.SetBool("IsAttacking", _isAttacking);
        }
    }
    
    void UpdateMovementInput()
    {
        if(_isAttacking)
        {
            _movementVelocity = Vector2.zero;

            return;
        }
        
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
        if (_isAttacking)
        {
            return;
        }
        
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
        if (_isAttacking && _isDead)
        {
            return;
        }
        
        if (_rigidbody)
        {
            _rigidbody.MovePosition(_rigidbody.position + _movementVelocity * Time.deltaTime);
        }
    }
    
    public void SetIsAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
        UpdateAttackAnimation();

        if(_isAttacking)
        {
            ActivateHitCollider();
        }
        else
        {
            DeactivateAllHitColliders();
        }
    }
    
    void ActivateHitCollider()
    {
        if(hitColliders.Count > (int)_movementDirection && hitColliders[(int)_movementDirection])
        {
            hitColliders[(int)_movementDirection].enabled = true;
        }
    }

    void DeactivateAllHitColliders()
    {
        for(int index = 0; index < (int)MoveDirection.MAX; ++index)
        {
            if (hitColliders.Count > index && hitColliders[index])
            {
                hitColliders[index].enabled = false;
            }
        }
    }
    
    public void TakeDamage()
    {
        if(_invincibilityTime > 0.0f)
        {
            return;
        }

        --_health;
        _health = Mathf.Clamp(_health, 0, MAX_HEALTH);

        UIManager.Instance.SetHealth(_health);
        _invincibilityTime = MAX_INVINCIBILITY_TIME;

        if (_health == 0)
        {
            Die();
        }
        else
        {
            PlaySound(gotHitSound);
        }
    }
    
    void Die()
    {
        _isDead = true;
        PlaySound(deathSound);
    }
    void PlaySound(AudioClip sound)
    {
        if(_audioSource && sound)
        {
            _audioSource.PlayOneShot(sound);
        }
    }
}
