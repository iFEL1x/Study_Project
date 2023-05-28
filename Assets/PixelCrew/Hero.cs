using UnityEngine;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private LayerMask _groundLayer;
    
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
    
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        
        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
        
            var isJumping = _direction.y > 0;
            if (isJumping)
            {
                if (IsGrounded() && _rigidbody.velocity.y <= 0.1f)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
                else if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
                }
                
            }
        }
    
        private bool IsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
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
    }
}
