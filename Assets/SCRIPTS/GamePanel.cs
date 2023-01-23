using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Runner;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Runner
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text leveltext;
        [SerializeField] private TMP_Text coinstext;

        [SerializeField] private TMP_Text leveltypetext;
        [SerializeField] private TMP_Text moneydonetext;
        
        [SerializeField] private TMP_Text recordtext;
        
        private Level _levelfile;
        private GameManager _gamemanagerfile;
        private PlayerController _playerfile;
        private MoneyCounter _moneycounterfile;
        
        private int _coinsfromgamamanager;
        private int _moneysum;

        // ----------------------------------

        private void Awake()
        {
            _gamemanagerfile = FindObjectOfType<GameManager>();
            
            // ??тут надо?
            _moneycounterfile = FindObjectOfType<MoneyCounter>();
            _gamemanagerfile.LevelChanged += PrintLevelText;
            _gamemanagerfile.CoinsEncreqased += PrintCoinsCount;

            //_playerfile = FindObjectOfType<PlayerController>();
            _moneysum = 0;
        }


        private void Start()
        {
            moneydonetext.text = "Money done: - ";
            _gamemanagerfile = FindObjectOfType<GameManager>();
            _moneycounterfile = FindObjectOfType<MoneyCounter>();
        }


        //--------------- методы печатают на UI -------------------------------------
        public void PrintLevelText()
        {
            _levelfile = FindObjectOfType<Level>();
            string whattoprintIntoLevel = _levelfile.currentlevel.ToString();

            leveltext.text = $"Number of try: {whattoprintIntoLevel} ";
            
            //--------- выводит тип (номер) уровня -----------------------------
              
            leveltypetext.text = $"Level {(_gamemanagerfile.leveltype + 1).ToString()}";

            
            recordtext.text = $"Record: {_moneycounterfile.record} $";
        }

        public void PrintCoinsCount()
        {
            _coinsfromgamamanager = _gamemanagerfile.howMuchCoins;
            coinstext.text = $"Coins collected: {_coinsfromgamamanager.ToString()}";

            //----------- ОШИБКА гдето осталась но хоть работает  -----------------------------------------------------
            //----------- сюда передавать сумму денег, или тут суммировать -----------------
            // ????????????????? ПОЛУЧАЕТСЯ В случае, где нам нужно "текущее состояние переменной в файле" - 
            // подключсаем его не в awake a в нужном месте функции??????
            //_playerfile = FindObjectOfType<PlayerController>();
            // или тут??
            //_moneycounterfile = FindObjectOfType<MoneyCounter>();
            _moneysum = _moneycounterfile.countmoney;
            //_moneysum = _playerfile.priceToAdd;
            moneydonetext.text = $"Money done: {_moneysum.ToString()} $";
            
            moneydonetext.text = $"Money done {_moneycounterfile.countmoney.ToString()} $";
            
        }

        

        private void OnDestroy()
        {
            _gamemanagerfile.LevelChanged -= PrintLevelText;
            _gamemanagerfile.CoinsEncreqased -= PrintCoinsCount;
        }
    }
}