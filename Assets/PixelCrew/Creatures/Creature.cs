using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _groundLayer;
        //[SerializeField] private LayerCheck _groundCheck;

    }
}