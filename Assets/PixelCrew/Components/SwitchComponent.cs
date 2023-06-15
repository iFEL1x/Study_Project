using UnityEngine;

namespace PixelCrew.Components
{
    public class SwitchComponent: MonoBehaviour

    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animatioinKey;

        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animatioinKey, _state);
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}