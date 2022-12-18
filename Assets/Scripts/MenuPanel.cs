using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    public class MenuPanel : PanelWithButton
    {
        public event Action OnStartGameButtonClick;

        protected override void OnButtonClick()
        {
            OnStartGameButtonClick?.Invoke();
        }
    }
}