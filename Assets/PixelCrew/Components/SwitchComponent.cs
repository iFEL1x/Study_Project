using System;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SwitchComponent: MonoBehaviour

    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animatioinKey;


        private void Start()
        {
            _animator.SetBool(_animatioinKey, _state);
        }

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