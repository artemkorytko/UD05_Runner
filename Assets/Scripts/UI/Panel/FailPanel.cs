using System;
using UnityEngine;

namespace Runner
{
    public class FailPanel : BasePanel
    {
        public event Action Restart;
        protected override void OnButtonClick()
        {
            Restart?.Invoke();
        }
    }
}