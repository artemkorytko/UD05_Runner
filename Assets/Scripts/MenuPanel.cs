using UnityEngine;
using System;

namespace Runner_
{
    public class MenuPanel : BasePanel
    {
        public event Action Start;
        protected override void OnClickButton()/*
        переопределение метода OnClickButton прописанного в базовом классе BasePanel
         (от которого наследуется текущий класс MenuPanel)*/
        {
            Start?.Invoke();
        }
    }
}