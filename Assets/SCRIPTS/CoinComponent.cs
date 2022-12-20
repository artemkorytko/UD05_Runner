using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class CoinComponent : MonoBehaviour
    {
        [SerializeField] public float coinrotSpeed = 1f; // в поле менять
        private PlayerController _playercontrollerfile;

        private void Start()
        {
            gameObject.SetActive(true); // #######################[ Я тут точно обращаюсь к самой монетке? ]
            _playercontrollerfile = FindObjectOfType<PlayerController>();
            _playercontrollerfile.GetCoin += DeleteCoin;
        }

        private void Update()
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0, coinrotSpeed, 0);
        }

        private void DeleteCoin(CoinComponent coin)
        {
            if (coin == this)
            {
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