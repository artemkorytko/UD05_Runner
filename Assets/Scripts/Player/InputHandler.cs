using System;
using UnityEngine;

namespace Runner
{
    public class InputHandler : MonoBehaviour
    {

        private float _prevPosX; // переменная для фиксации(записи) где была нажата ЛКМ 
        private float _relativeOffset; // переменная для того чтоб не было разницы смещения (влево/враво), в зависимости от ширины экрана девайса) т.е идет "уравнение(24д = 8д)" экранов 
        private bool _isHold; // флаг для замены Input.GetMouseButton(0) 3ий if в Update
        private int _screenWidth; // переменная для записи ширины эрана. 

        public float HorizontalAxis => _relativeOffset; // свойство для передачи _relativeOffset в PlayerController, в момент когда произошло изменение переменной (_relativeOffset)

        private void Awake()
        {
            _screenWidth = Screen.width; // записm ширины эрана.
        }

        private void Update() // считывание того что было нажато 
        {
            if (Input.GetMouseButtonDown(0)) // когда ЛКМ была нажата (1(кадр) раз отработает)
            {
                _isHold = true; // активирует 3ий if
                _prevPosX = Input.mousePosition.x; // запись позиции мышки по коордитате "X"
            }

            if (Input.GetMouseButtonUp(0)) // когда ЛКМ была отпущена (1(кадр) раз отработает)
            {
                _isHold = false; // деактивирует 3ий if
                _prevPosX = 0; // сброс позиции мышки по коордитате "X" в 0
            }

            if (_isHold) // замена (GetMouseButton(0) - метод отрабатывает(дохрена раз) когда ЛКМ была зажата) (if сделан через флаг для оптимизации)
            {
                var mousePos = Input.mousePosition.x; // запись позиции мышки по коордитате "X"(тип сделали кэширование т.к к Input.mousePosition.x обращаемся 2 раза)
                var offset = _prevPosX - mousePos; // разница между предыдушим кадром и текущим кадром по коордитате "X" от -(лево) до +(право) т.е определяет напровление смещения Player'a

                _relativeOffset = offset / _screenWidth; // происходит "уравнение(24д = 8д)" экранов в зависимости от оффсет'a 
                _prevPosX = mousePos;
            }
        }
    }
}