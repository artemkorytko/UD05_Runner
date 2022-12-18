using System;
using UnityEngine;

namespace Runner
{
    public class FailPanel : PanelWithButton
    {
        public event Action RestartLevelButtonClick;

        protected override void OnButtonClick()
        {
            RestartLevelButtonClick?.Invoke();
        }
    }
}