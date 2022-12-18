using System;

namespace Runner
{
    public class MenuPanel : PanelWithButton
    {
        public event Action OnStartButtonClick;

        protected override void OnButtonClick()
        {
            OnStartButtonClick?.Invoke();
        }
        
    }
}