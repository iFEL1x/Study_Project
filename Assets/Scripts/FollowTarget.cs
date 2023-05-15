using UnityEngine;


namespace PixelCrew
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _damping;
        
        private void LateUpdate()
        {
            var destination = new Vector3(_target.position.x, _target.position.y, _target.position.z);
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * _damping);
        }
    }
}

