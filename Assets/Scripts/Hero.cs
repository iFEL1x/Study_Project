using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _direction;

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

    public void SaySomething()
    {
        Debug.Log("Something!");
    }
}
