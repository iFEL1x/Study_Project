using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")] 
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float JumpSpeed;
        [SerializeField] private float _damageVelocity;
        
        [Header("Checkers")]
        [SerializeField] protected LayerMask GroundLayer;
        [SerializeField] protected LayerCheck GroundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent Particles;

        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected bool IsGrounded;
        private bool _isJumping;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }
        
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = GroundCheck.IsTouchingLayer;
        }
        
        private void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetBool(IsRunning, Direction.x != 0);
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
            
            UpdateSpriteDirection();
        }
        
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;
            
            if (IsGrounded)
            {
                _isJumping = false;
            }
            if (isJumpPressing)
            {
                _isJumping = true;
                
                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }
            
            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity = JumpSpeed;
                Particles.Spawn("Jump");
            } 
            
            return yVelocity;
        }
        
        protected void UpdateSpriteDirection()
        {
            var multipler = _invertScale ? -1 : 1;
            if (Direction.x > 0)
                transform.localScale = new Vector3(multipler,1,1);
            else if (Direction.x < 0)
                transform.localScale = new Vector3(-1 * multipler, 1, 1);
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
        }
        
        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }
        
        public void Attacking()
        {
            _attackRange.Check();
        }
    }
}