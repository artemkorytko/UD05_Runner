using System;

namespace Runner
{
    public class WinPanel : PanelWithButton
    {
        public event Action OnNextButtonClick;

        protected override void OnButtonClick()
        {
            OnNextButtonClick?.Invoke();
        }
    }
}