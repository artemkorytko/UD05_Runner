using System;
using UnityEngine;

namespace Runner
{
    public class MenuPanel : BasePanel
    {
        public event Action Start;
        protected override void OnButtonClick()
        {
            Start?.Invoke();
        }
    }
}