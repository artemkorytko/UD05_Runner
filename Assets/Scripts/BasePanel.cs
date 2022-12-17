using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner_
{
    public abstract class BasePanel : MonoBehaviour
    {
        private Button _button;


        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClickButton); //onClick.AddListener -подписка на событие (=>если есть согбытие и подписка, значит нужна отпсика)
            
        }

        private void OnDestroy()
        { 
            _button.onClick.RemoveListener(OnClickButton); //RemoveListener - отписка от события  OnClickButton
            
        }

        protected abstract void OnClickButton(); //protected - защищенный, им могут пользоваться только унаследованные 
        
    }
}