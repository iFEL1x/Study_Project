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
            _inputAtcions.Hero.SaySomething.performed += OnSaySomething;
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

        private void OnSaySomething(InputAction.CallbackContext context)
        {
            _hero.SaySomething();
        }
    }
}

