using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Spawning,
    Attacking
}
public class EnemyLogic : MonoBehaviour
{
    EnemyState _enemyState = EnemyState.Spawning;

    GameObject _player;

    Vector2 _attackDirection;

    float _movementSpeed = 5.0f;
    
    Rigidbody2D _rigidbody;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_enemyState == EnemyState.Attacking)
        {
            if(_rigidbody)
            {
                _rigidbody.MovePosition(_rigidbody.position + (_attackDirection * _movementSpeed * Time.deltaTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _attackDirection = -_attackDirection;
        
        Rigidbody2D rigidBody = collision.rigidbody;
        if(rigidBody && rigidBody.tag == "Player")
        {
            PlayerLogic playerLogic = rigidBody.GetComponent<PlayerLogic>();
            if(playerLogic)
            {
                playerLogic.TakeDamage();
            }
        }
    }
    
    public void SetEnemyState(EnemyState enemyState)
    {
        _enemyState = enemyState;

        if(_enemyState == EnemyState.Attacking)
        {
            DetermineAttackDirection();
        }
    }

    void DetermineAttackDirection()
    {
        if(!_player)
        {
            return;
        }

        _attackDirection = _player.transform.position - transform.position;
        _attackDirection.Normalize();
    }
}
