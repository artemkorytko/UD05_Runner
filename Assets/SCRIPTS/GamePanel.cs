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
            _moneycounterfile = FindObjectOfType<MoneyCounter>();

            _gamemanagerfile.LevelChanged += PrintLevelText;
            _gamemanagerfile.CoinsEncreqased += PrintCoinsCount;
            
            _moneycounterfile.NewRecordMade += ShowNewRecord;
            _moneysum = 0;
        }


        private void Start()
        {
             moneydonetext.text = "Money done: - ";
            _gamemanagerfile = FindObjectOfType<GameManager>();
            _moneycounterfile = FindObjectOfType<MoneyCounter>();
            
            // NEW - всегда выводит рекорд в начале игры
            recordtext.text = $"Top record: ${_moneycounterfile.record}";
        }


        //--------------- методы печатают на UI -------------------------------------
        public void PrintLevelText()
        {
            _levelfile = FindObjectOfType<Level>();
            string whattoprintIntoLevel = _levelfile.currentlevel.ToString();

            leveltext.text = $"Number of try: {whattoprintIntoLevel} ";
            
            //--------- выводит тип (номер) уровня -----------------------------
            leveltypetext.text = $"Level {(_gamemanagerfile.leveltype + 1).ToString()}";
            
            recordtext.text = $"Top record: ${_moneycounterfile.record}";
            
            // new - шоб после упс при перезапуске уровня были нолики
            PrintCoinsCount();
        }

        public void PrintCoinsCount()
        {
            _coinsfromgamamanager = _gamemanagerfile.howMuchCoins;
            coinstext.text = $"Coins collected: {_coinsfromgamamanager.ToString()}";
            
            _moneysum = _moneycounterfile.countmoney;
            moneydonetext.text = $"Money done: ${_moneysum.ToString()}";
        }

        private void ShowNewRecord() // вызывается когда добежали и поставили рекорд
        {
            recordtext.text = $"Top record: ${_moneycounterfile.record}";
        }

        public void OopsifDie()
        {
            // Debug.Log("Панель знает что треснулся");
            moneydonetext.text = " Oooops! ";
            coinstext.text = " Oooops! ";
        }

        private void OnDestroy()
        {
            _gamemanagerfile.LevelChanged -= PrintLevelText;
            _gamemanagerfile.CoinsEncreqased -= PrintCoinsCount;
            
            _moneycounterfile.NewRecordMade -= ShowNewRecord;
            
            _playerfile.OnDie -= OopsifDie;
        }
    }
}