using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// ------- скрипт на все такие проекты где пальцем свайп
// ------- добавить скрипт компонентом на персонажа
namespace Runner
{


    public class InputHandler : MonoBehaviour
    {
        
        private bool _isHold;
        private float _prevPosX;
        private float _relativeOffset;

        private int _screenWidth = Screen.width; // получаем ширину экрана
        
        
        // показать наружу
        public float HorizontalAxis => _relativeOffset;
        
        
        
        
        private void Update()
        {    
            if (Input.GetMouseButtonDown(0)) // --- 0 - левая кнопка или первый тач!!! 1- правая, 2 - колесо
            {
                _isHold = true; //Input.GetTouch(0).phase // ПОЧИТАТЬ!!!!!!!!!!!!!! там дофига нужного
                _prevPosX = Input.mousePosition.x;
                
            }
            
            
            if (Input.GetMouseButtonUp(0)) // --- 0 - левая кнопка или первый тач!!! 1- правая, 2 - колесо
            {
                _isHold = false;
                _prevPosX = 0;
            }
            
            
            // каждый кадр пока мышка удержана - между опусканием и поднятием
            if (_isHold) // оптимизация - шоб не ходить в движок больше 2 раз
            {
                var mousePos = Input.mousePosition.x; // 1 раз получаем изнутри движка
                // позиция по х
                // если больше предыдущего кадра то мы шли вправо
                // ищем разницу будет - либо + (право) / либо -- (минус)
                var raznitsa = _prevPosX - mousePos;
                
                // экраны разные! палец относительно экрана разно свайпает - уравниваем
                _relativeOffset = raznitsa / _screenWidth;

                _prevPosX = mousePos;
            }
        }
    }
}