using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// ------- этот скрипт !!! можно использовать во всех проектах, где пальцем свайп
// ------- добавить скрипт компонентом на персонажа
namespace Runner
{


    public class InputHandler : MonoBehaviour
    {
        
        private bool _isHold; // переменная, нажали ли мышку / экран
        private float _prevPosX;
        private float _relativeOffset; // для разницы свайпов на разных эеранах

        private int _screenWidth = Screen.width; // получаем ширину экрана 1 раз тут в начале
        
        
        // показать наружу HorizontalAxis наш унифицированный оффсет - будет использовтаься в PlayerController, Move()
        public float HorizontalAxis => _relativeOffset;

        public bool IsHold => _isHold; //выносим наружу для починки поворота
        
        
        private void Update() // тут считываем то что было нажато
        {    
            // ------ ТРИ состояния взаимодействия с экраном/мышкой:
            
            // ----- 1 ------------------------------------------------
            // ----- это 1й кадр когда коснулись экрана
            // с мультитатчем заводить Input.GetTouch(0).phase // ПОЧИТАТЬ!!!!!!!!!!!!!! там дофига нужного
            if (Input.GetMouseButtonDown(0)) // --- 0 - левая кнопка / первый тач!!! 1- правая, 2 - колесо
            {
                _isHold = true; //Input.GetTouch(0).phase // ПОЧИТАТЬ!!!!!!!!!!!!!! там дофига нужного
                _prevPosX = Input.mousePosition.x;
                
            }
            
            // ----- 2 --------
            // кадр когда оторвали палец от экрана
            if (Input.GetMouseButtonUp(0)) // --- 0 - левая кнопка или первый тач!!! 1- правая, 2 - колесо
            {
                _isHold = false;
                _prevPosX = 0;
            }
            
            // ----- 3 --------
            // каждый кадр пока мышка удержана - состояние между опусканием и поднятием
            if (_isHold) // оптимизация - шоб не ходить в движок больше 2 раз через "Input.GetMouseButton"
            {    
                // записали позицию мышки по х, тут записали 1 раз - ниже используется 2 раза
                var mousePos = Input.mousePosition.x; 
                
                // если больше предыдущего кадра то мы шли вправо
                // ищем разницу будет: либо + (право) / либо -- (минус)
                var raznitsa = _prevPosX - mousePos;
                
                // экраны разные! палец относительно экрана разно свайпает - уравниваем
                // маленький свайп на маленьком экране
                _relativeOffset = raznitsa / _screenWidth;

                // обновляем предыдущую позицию на текущую позицию
                _prevPosX = mousePos;
               
            }
            
            // * Курочка по зёрнышку - весь двор в говне *
        }
    }
}