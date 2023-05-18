using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    private Rigidbody2D _rigidbody;
    private float _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        if (_direction != 0)
        {
            var delta = _direction * _speed * Time.deltaTime;
            var newXposition = transform.position.x + delta;
            transform.position = new Vector3(newXposition, transform.position.y, transform.position.z);
        }
    }

    // private void FixedUpdate()
    // {
    //     _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
    //
    //     var isJumping = _direction.y > 0;
    //     if (isJumping)
    //     {
    //         _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
    //     }
    // }

    public void SaySomething()
    {
        Debug.Log("Something!");
    }
}
