using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _aplhTime = 1;
        [SerializeField] private float _moveTime = 1;
        
        public void Teleport(GameObject target)
        {
            StartCoroutine(AnimationTeleport(target));
        }

        private IEnumerator AnimationTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            //var input = target.GetComponent<PlayerInput>();
            
            //SetLockInput(input, false);
            yield return AlphaAnimation(sprite, 0f);
            //target.SetActive((false));

            yield return MoveAnimation(target);
            
            //target.SetActive(true);
            yield return AlphaAnimation(sprite, 1f);
            //SetLockInput(input, true);
        }

        // private void SetLockInput(PlayerInput input, bool isLocked)
        // {
        //     if (input != null)
        //     {
        //         Debug.Log(true);
        //         input.enabled = isLocked;
        //     }
        // }
        
        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

                yield return null;
            }
        }
        
        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
           
            var time = 0f;
            var spriteAlpha = sprite.color.a;
            while (time < _aplhTime)
            {
                time += Time.deltaTime;
                var progress = time / _aplhTime;
                var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                yield return null;
            }
        }
    }
}