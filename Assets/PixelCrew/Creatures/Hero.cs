using System;
using PixelCrew.Components;
using PixelCrew.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : Creature
    {
        [SerializeField] private float _damageDownVelocity;
        [SerializeField] private int _damageIsFall;
        
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _interactionRadius;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;
        
        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem _hitCointParticles;
        
        private HealthComponent _healthComponent;
        private bool _allowDoubleJump;
        private bool _isOnWall;
        
        private GameSession _session;
        private float _defaultGravityScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _healthComponent.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        protected override void Update()
        {
            base.Update();

            if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;
            
            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump)
            {
                Particles.Spawn("Jump");
                _allowDoubleJump = false;
                return JumpSpeed;
            }
            
            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} coins added. total coins:{_session.Data.Coins}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var _numCoinsToDispose = Math.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= _numCoinsToDispose;

            var burst = _hitCointParticles.emission.GetBurst(0);
            burst.count = _numCoinsToDispose;
            _hitCointParticles.emission.SetBurst(0, burst);
            
            _hitCointParticles.gameObject.SetActive(true);
            _hitCointParticles.Play();
            Debug.Log($"Total coins:{_session.Data.Coins}");
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(GroundLayer))
            {
                var contact = other.contacts[0];
                
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    Particles.Spawn("SlamDown");
                }

                if (contact.relativeVelocity.y >= _damageDownVelocity)
                {
                    _healthComponent.ModifyHealth(-_damageIsFall);
                }
            }
        }

        public override void Attack()
        {
            if(!_session.Data.IsArmed) return;
            base.Attack();
        }
        
        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }
    }
}