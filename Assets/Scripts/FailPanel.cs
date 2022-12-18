using System;

namespace Runner
{
    public class FailPanel : PanelWithButton
    {
        public event Action OnNextButtonClick;

        protected override void OnButtonClick()
        {
            OnNextButtonClick?.Invoke();
        }
    }
}