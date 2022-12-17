using UnityEngine;
using System;

namespace Runner_
{
    public class WinPanel : BasePanel
    {
        public event Action NextLevel;
        protected override void OnClickButton() /*
        переопределение метода OnClickButton прописанного в базовом классе BasePanel
         (от которого наследуется текущий класс WinPanel)*/
        {
            NextLevel?.Invoke();
        }
    }
}