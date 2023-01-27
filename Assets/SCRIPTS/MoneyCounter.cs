using System;
using Unity.VisualScripting;
using UnityEngine;

//------------- калькулятор внутриигрового бабла ----------------
namespace Runner
{
    public class MoneyCounter : MonoBehaviour
    {
        // для сохранения:
        private const string SAVE_KEY = "Record";

        // переменная, чтобы считать сколько заработали в сумме денежек
        public int countmoney = 0;
        
        // переменная для вывода и хранения рекорда
        public int record;
        
        private PlayerController _playerContfile;
        
        public event Action NewRecordMade; //---- идет в геймпанель отображать рекорд сразу, пока чел танцует

        private void Awake()
        {
            LoadData(); //NEW - при первом запуске идет cxитывает топ рекорд
        }

        public void InitMoneyCounter()
        {
            _playerContfile = FindObjectOfType<PlayerController>();
            
            _playerContfile.GetMoney += AddMoney;
            _playerContfile.Dobezal += SaveData;
            _playerContfile.OnDie += ObnulenujeBabla;
            LoadData();
        }

        private void AddMoney(int moneycame)
        {
            countmoney += moneycame; //игрок взял монетку - увеличили переменную
            
            Debug.Log($"   увеличили сумму - стало {countmoney}");
        }


        //-------------- сохранение рекорда---------------------------
        private void SaveData()
        {
            if (countmoney > record) // если текущее больше рекорда    && countmoney != 0
            {
                record = countmoney;
                PlayerPrefs.SetInt(SAVE_KEY, record);
                Debug.Log("   record saved");
                NewRecordMade?.Invoke(); //---- идет в геймпанель отображать рекорд сразу пока танцует
            }
            else return;
        }

        private void LoadData()
        {
            record = PlayerPrefs.GetInt(SAVE_KEY, 0);
            Debug.Log("   record loaded");
        }

        //--------- если врезался - обнуляет деньги ------------------
        private void ObnulenujeBabla()
        {
            countmoney = 0;
            
            // Вот тут надо MoneyDone тоже обнулить
        }

        private void OnDestroy()
        {
            _playerContfile.GetMoney -= AddMoney;
            _playerContfile.Dobezal -= SaveData;
            _playerContfile.OnDie -= ObnulenujeBabla;
        }
    }
}