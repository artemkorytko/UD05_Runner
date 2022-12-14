﻿using System;
using UnityEngine;

namespace Runner
{
    public class InputHandler : MonoBehaviour
    {
        private bool _isHold;
        private float _prevPosX;
        private float _relativeOffset;
        private int _screenWidth;

        public float HorizontalAxis => _relativeOffset;

        private void Awake()
        {
            _screenWidth = Screen.width;
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
            
            if (_isHold)
            {
                var mousePos = Input.mousePosition.x;
                var offset = _prevPosX - mousePos;
                _relativeOffset = offset / _screenWidth;
                _prevPosX = mousePos;
            }
        }
    }
}