using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Runner;
using TMPro;
using UnityEngine;

namespace Runner
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text leveltext;
        [SerializeField] private TMP_Text coinstext;

        private Level _levelfile;
        private GameManager _gamemanagerfile;
        private int _coinsfromgamamanager;
        

        // ----------------------------------

        private void Awake()
        {
            _gamemanagerfile = FindObjectOfType<GameManager>();
            _gamemanagerfile.LevelChanged += PrintLevelText;

            _gamemanagerfile.CoinsEncreqased += PrintCoinsCount;
            
            
            //---- почему не работает? :/ Панель создается раньше человека??
            // _playercontrollerfile = FindObjectOfType<PlayerController>();
            // _playercontrollerfile.GetCoin += PrintCoinsCount;

        }



        //--------------- методы печатают на UI -------------------------------------
        public void PrintLevelText()
        {
            _levelfile = FindObjectOfType<Level>();
            string whattoprintIntoLevel = _levelfile.currentlevel.ToString();

            leveltext.text = $"Kozel: {whattoprintIntoLevel} ";
        }

        public void PrintCoinsCount()
        {
            _coinsfromgamamanager = _gamemanagerfile.howMuchCoins;
            coinstext.text = $"Coins collected: {_coinsfromgamamanager.ToString()}";
        }


        private void OnDestroy()
        {
            _gamemanagerfile.LevelChanged -= PrintLevelText;
            _gamemanagerfile.CoinsEncreqased -= PrintCoinsCount;
            
            //_playercontrollerfile.GetCoin -= PrintCoinsCount; - не работало :/

            // soMuchCoins = 0; ---- пока не обнуляем монетки
        }
    }
}