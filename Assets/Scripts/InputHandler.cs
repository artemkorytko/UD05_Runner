using System;
using UnityEngine;

namespace Runner_
{
    public class InputHandler : MonoBehaviour //здесь реализовано считывание движения вправо влево по экрану (свайпом/мышкой)
    {
        private bool _isHold; //прописываем в методах и присваиваем переменной тру или фолс (тру если удерживаем, фолс - не удерживаем)
        private float _prevPosX; //значение предыдущей позиции по иксу
        private float _relativeOffset;

        private int _screenWidth; //ширина экрана (она конст)

        public float HorizontalAxis => _relativeOffset;
        public bool IsHold { get; set; }


        private void Awake()
        {
            _screenWidth = Screen.width; //ширина экрана которая постоянна ( т к не меняется ширина моей мобилы) а Awake вызывается один раз
        }

        private void Update()  //3 фазы
        {
            if (Input.GetMouseButtonDown(0)) //Input.GetMouseButtonDown(0) - левая кнопка мыши или первый тач// 0 -нулевой тач // вызывается один раз (т е самый первый момент зажатия мышки)
            {
                _isHold = true;
                _prevPosX = Input.mousePosition.x; //вернули значение по иксу в котором только-только клинклуи (т е запоминаем координату когда случился клик (0 момента при клике) ((который будет продолжительным)))
            }
            if (Input.GetMouseButtonUp(0)) //кдр оторвали палец от первого клика/тача //отработает один раз (т е самой последний момент окончания клика)
            {
                _isHold = false;
                _prevPosX = 0;

            }

            if (_isHold) /*срабатывает пока мышка не отпущена//удержание мышки - это состояние между тачем и 
            подянтием // _isHold - булевская переменная прописанная выше (т е зайдет в иф если _isHold тру) */
            {
                var mousePos = Input.mousePosition.x; // в mousePos запишется нынешнее положение мышки
                var offset = _prevPosX - mousePos; //разница между предыдущим положением и нынешним, если результат > 0 => сдвтнулись влево
                _relativeOffset = offset / _screenWidth; // считаем относительную ширину экрана ( = нашему смещению / на ширину экрана) чтобы уравнять свайп и работало и на больших и на маленьких экрананх корректно(чтобы свайп был пропорциональный на разных экранах по ширине)
                _prevPosX = mousePos;
            } 
        }
        
    }
}