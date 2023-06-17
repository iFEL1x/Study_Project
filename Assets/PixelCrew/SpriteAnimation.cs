using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [Range(1, 60)]
        [SerializeField] private int _frameRate;
        [SerializeField] private bool _loop;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSprtireIndex;
        private float _nextFrameTime;
        
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentSprtireIndex = 0;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            if(_currentSprtireIndex >= _sprites.Length)
            {
                if (_loop)
                {
                    _currentSprtireIndex = 0;
                }
                else
                {
                    enabled = false;
                    _onComplete?.Invoke();
                    return;
                }
            }
            
            _renderer.sprite = _sprites[_currentSprtireIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSprtireIndex++;
            
        }
    }
}


