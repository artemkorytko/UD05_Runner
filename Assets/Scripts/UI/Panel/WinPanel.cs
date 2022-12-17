﻿using System;
using UnityEngine;

namespace Runner
{
    public class WinPanel : BasePanel
    {
        public event Action PlayNextLevel;
        protected override void OnButtonClick()
        {
            PlayNextLevel?.Invoke();
        }
    }
}