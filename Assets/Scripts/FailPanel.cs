using UnityEngine;
using System;


namespace Runner_
{
    public class FailPanel : BasePanel
    {
        public event Action Restart;/*ивент о клике на кнопку когда мы находимся на ФэйлПанеле (чтобы по итогу сделать рестарт 
        и перейти заново на ГеймПанел и тот же уровень  с которого мы умерли)
        */
        protected override void OnClickButton() /*
        переопределение метода OnClickButton прописанного в базовом классе BasePanel
         (от которого наследуется текущий класс FailPanel)*/
        {
            Restart?.Invoke();  //инвоук отработает в ЮайКонтроллере
        }
    }
}