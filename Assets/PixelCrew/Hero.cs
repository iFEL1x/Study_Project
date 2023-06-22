using System;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
    
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _footJumpParticles;
        [SerializeField] private SpawnComponent _footFallParticles;
        [SerializeField] private ParticleSystem _hitCointParticles;
        
        private Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;
        private bool _longFall;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");

        private int _coins;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
    
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);
            
            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            if (!_longFall && yVelocity < -12f) _longFall = true;
            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.0001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                SpawnJumpDust();
            } else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                SpawnJumpDust();
            }
            return yVelocity;
        }
        
        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
                transform.localScale = Vector3.one;

            else if (_direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    
        private bool IsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, 
                _groundCheckRadius, Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);
        }
    
        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log($"{coins} coins added. total coins:{_coins}");
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

            if (_coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var _numCoinsToDispose = Math.Min(_coins, 5);
            _coins -= _numCoinsToDispose;

            var burst = _hitCointParticles.emission.GetBurst(0);
            burst.count = _numCoinsToDispose;
            _hitCointParticles.emission.SetBurst(0, burst);
            
            _hitCointParticles.gameObject.SetActive(true);
            _hitCointParticles.Play();
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position, 
                _interactionRadius, 
                _interactionResult, 
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        private void SpawnJumpDust()
        {
            _footJumpParticles.Spawn();
        }

        public void SpawnFallDust()
        {
            if (!_longFall) return;
            
            _footFallParticles.Spawn();
            _longFall = false;
        }
    }
}