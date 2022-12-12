using System;
using UnityEngine;

namespace Runner_
{
    public class InputHandler : MonoBehaviour
    {
        private bool _isHold;
        private float _prevPosX; //значение предыдущей позиции по иксу
        private float _relativeOffset;

        private int _screenWidth;

        public float HorizontalAxis => _relativeOffset;
        

        private void Awake()
        {
            _screenWidth = Screen.width; //ширина экрана которая постоянна ( т к не меняется ширина моей мобилы)
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) //Input.GetMouseButtonDown(0) - левая кнопка мыши или первый тач
            {
                _isHold = true;
                _prevPosX = Input.mousePosition.x; //вернули значение по иксу
            }
            if (Input.GetMouseButtonUp(0)) //кдр оторвали палец от первого клика/тача
            {
                _isHold = false;
                _prevPosX = 0;

            }

            if (_isHold) //удержание мышки - это состояние между тачем и подянтием 
            {
                var mousePos = Input.mousePosition.x;
                var offset = _prevPosX - Input.mousePosition.x; //разница между предыдущим положением и нынешним, если результат > 0 => сдвтнулись влево
                _relativeOffset = offset / _screenWidth;
                _prevPosX = mousePos;
            } 
        }
        
    }
}