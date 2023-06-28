using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        [ContextMenu("Invoke Interact")]
        public void Interact()
        {
            Debug.Log("Interact");
            _action.Invoke();
        }
    }
}