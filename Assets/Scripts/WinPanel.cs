using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Runner
{
    public class WinPanel : PanelWithButton
    {
        public event Action NextLevelButtonClick;

        protected override void OnButtonClick()
        {
            NextLevelButtonClick?.Invoke();
        }
    }
}