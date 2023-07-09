using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private HeroInputAction _inputAtcions;
        private void Awake()
        {
            _inputAtcions = new HeroInputAction();
            _inputAtcions.Hero.HorisontalMovement.performed += OnHorisontalMovement;
            _inputAtcions.Hero.HorisontalMovement.canceled += OnHorisontalMovement;
            _inputAtcions.Hero.Interact.canceled += OnInteract;
            _inputAtcions.Hero.Attack.canceled += OnAttack;
            _inputAtcions.Hero.Throw.performed += OnThrow;
        }

        private void OnEnable()
        {
            _inputAtcions.Enable();
        }

        private void OnHorisontalMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (_hero != null)
                    _hero.Interact();
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                if (_hero != null)
                    _hero.Attack();
            }
        }

        private void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Throw();
            }
        }
    }
}

