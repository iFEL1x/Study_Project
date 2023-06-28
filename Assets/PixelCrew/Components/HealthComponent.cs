using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;
        private bool _isDiead;

        public void ModifyHealth(int healthDelta)
        {
            _health += healthDelta;
            _onChange?.Invoke(_health);
            
            if (_health > 0)
            {
                if (healthDelta < 0)
                {
                    _onDamage?.Invoke();
                }
                else if (healthDelta > 0)
                {
                    _onHeal?.Invoke();
                }
            }
            else if (_health <= 0 && !_isDiead)
            {
                _onDie?.Invoke();
                _isDiead = true;
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }
        
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
        }
    }
}