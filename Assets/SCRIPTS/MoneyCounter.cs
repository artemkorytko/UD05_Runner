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

        // переменная чтобы считать сколько заработали в сумме денежек
        public int countmoney  = 0; 
        private PlayerController _playerContfile;
        public int record;
        

        private void Start()
        {
            // пока не вынесла в старт - не мог найти)))
            _playerContfile = FindObjectOfType<PlayerController>();
            _playerContfile.GetMoney += AddMoney;
            _playerContfile.Dobezal += SaveData;
            _playerContfile.OnDie += ObnulenujeBabla;
            LoadData();
            record = 0;
        }

        private void AddMoney(int moneycame)
        {   
            _playerContfile = FindObjectOfType<PlayerController>();
            countmoney += moneycame; //игрок взял монетку - увеличили переменную
            
            // !!!!! ошибка на втором уровне уже не работает !!!!!!
            Debug.Log($"   увеличили сумму - стало {countmoney}");
        }

        
        //-------------- сохранение рекорда---------------------------
        private void SaveData()
        {
            if (countmoney > record ) // если текущее больше рекорда    && countmoney != 0
            {
             PlayerPrefs.SetInt(SAVE_KEY, record);
             Debug.Log("   record saved");
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
        }

        private void OnDestroy()
        {
            _playerContfile.GetMoney -= AddMoney;
        }
    }
}