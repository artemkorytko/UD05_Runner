using System;
using UnityEngine;

namespace Runner
{
    public class InputHandler : MonoBehaviour
    {
        private bool _isHold;
        private float _prevPosX;
        private float _realativeOffset;

        private int _screenWith;

        public float HorizontalAxis => _realativeOffset;

        private void Awake()
        {
            _screenWith = Screen.width;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isHold = true;
                _prevPosX = Input.mousePosition.x;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isHold = false;
                _prevPosX = 0;
            }

            if (_isHold)  // замена GetMouseButton
            {
                var mousePos = Input.mousePosition.x;
                var offset = _prevPosX - mousePos;
                
                _realativeOffset = offset / _screenWith;
                _prevPosX = mousePos;
            }
            
        }
    }
}