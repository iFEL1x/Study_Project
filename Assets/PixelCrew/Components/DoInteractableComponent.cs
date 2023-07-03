using UnityEngine;

namespace PixelCrew.Components
{
    public class DoInteractableComponent : MonoBehaviour
    {
        public void DoInteractable(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();
            if (interactable != null)
                interactable.Interact();
        }
    }
}