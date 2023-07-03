using System.Collections;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class MobAi : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        private Coroutine _current;
        private GameObject _target;

        private SpawnListComponent _particles;
        private Creature _creature;

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
        }

        private void Start()
        {
            StartState(Patrolling());
        }

        public void OnHeroInVision(GameObject go)
        {
            _target = go;
            StartState(AgroToHero());
        }
        
        IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }
        
        IEnumerator GoToHero()
        {
            while(_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectiobToTarget();
                }
                
                yield return null;
            }
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            
            StartState(GoToHero());
        }

        private void SetDirectiobToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }

        private IEnumerator Patrolling()
        {
            yield return null;
        }
        
        private void StartState(IEnumerator coroutine)
        {
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }
    }
}