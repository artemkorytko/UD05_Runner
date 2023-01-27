using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Runner
{
    public class CoinComponent : MonoBehaviour
    {
        // было до конфигов:
        //[SerializeField] public float coinrotSpeed = 1f; // в поле менять
        private float _coinrotSpeed;
        private PlayerController _playercontrollerfile;
        private GameManager _gameManager;

        [SerializeField] public CoinConfig _thiscoinconfig;
        public int thisCoinPrice;
        
        private void Awake()
        {
            _playercontrollerfile = FindObjectOfType<PlayerController>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            gameObject.SetActive(true); // [ Я тут точно обращаюсь к самой монетке? ]
            _playercontrollerfile.GetCoin += DeleteCoin;
            
            // к конфигу обращаемся через объект, на котором висит контейнер конфигов?
            _coinrotSpeed = _gameManager.container.coinrotSpeed;
            
            // NEW
            // из навешенного руками конфига конкретной монетки цена пойдет
            // внутрь MoneyCounter по событию GetMoney e плеера
            // это работает!
            thisCoinPrice = _thiscoinconfig.PointNumber;
        }

        private void Update()
        {
            // монетка крутится
            transform.rotation = transform.rotation * Quaternion.Euler(0, _coinrotSpeed, 0);
        }

        private void DeleteCoin(CoinComponent coin) // так это идет ИЗ пллера дааа???
        {
            if (coin == this)
            {
                //было и работает:
                gameObject.SetActive(false);
            }
            //Destroy(transform.GetChild(i).GameObject()); - попробовать что ли
        }

        private void OnDestroy()
        {
            _playercontrollerfile.GetCoin -= DeleteCoin;
        }
    }
}