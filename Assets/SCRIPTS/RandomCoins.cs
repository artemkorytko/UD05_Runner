using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


//--------------- теперь это скорее рандом левелс -------------
namespace Runner
{
    // допустим, три монетки с весами 10, 40, 80
    public class RandomCoins : MonoBehaviour
    {
        //public GameObject giveRndCoin;

        private CoinConfig _thiscoin;
        
        //private Level _levelfile;

        //[SerializeField] private RandomCoinsClass[] cointypesarray;
        //[SerializeField] private GameConfigsContainer coinconfigsinlevel;
        [SerializeField] private CoinsOfLevelConfig _coinsOfLevelConfigfile;

            private void Awake()
        {
           // coinconfigsinlevel = FindObjectOfType<GameConfigsContainer>();
           //_coinsOfLevelConfigfile = FindObjectOfType<CoinsOfLevelConfig>();
        }

        //private GameObject _currentobject;
        

        public CoinConfig GetOneCoin()
        { 
            _thiscoin = _coinsOfLevelConfigfile.GoGetCoin();
           
            // ПРОБЛЕМА ТУТ
            
            //coinconfigsinlevel.GetCoinsOfLevelConfig(ConfigType.Easy).GoGetCoin();
            return _thiscoin;
        }

        //------ функция-прокладка, которую вызывает Level в момент генерации монетки ----
        // public GameObject GoGenerateCoin()
        // {
        //     return GetCoin().CoinPrefab;
        //     
        // }

        // public RandomCoinsClass GoGenerateCoin()
        // {
        //     RandomCoinsClass returncoin = new RandomCoinsClass();
        //     return GetCoin();
        // }
        
        // public RandomCoinsClass GoGenerateCoin()
        // {
        //     RandomCoinsClass returncoin = new RandomCoinsClass();
        //     return GetCoin();
        // }
        
        //------- ???????????????????????????????????? ------------------
        // void не возвращает ничего
        // а эта функция возвращает.....объект(??) класса RandomCoinsClass
        // public CoinConfigsAll GetCoin()
        // {
        //     
        //     // первый прогон массива для получения суммы весов
        //     int total = 0;
        //     foreach (var item in coinconfigsinlevel)
        //     {
        //         total += item.CoinWeight; // 10, 40, 80
        //     } // по итогу 130
        //     
        //     // второй прогон массива с рандомом и сравнением 
        //     var rnd = Random.Range(0, total); // дает число от 0 до 130
        //     var current = 0;
        //     foreach (var item in coinconfigsinlevel) //для каждого типа монетки
        //     {
        //         //переменная current будет 10, 50, 120
        //         current = current + item.CoinWeight; //или  сurrent += item.CoinWeight
        //
        //         // 55 <= 10,   89 <= 40,  7 <= 80    
        //         if (rnd <= current)
        //         {
        //             return item;
        //         }
        //     }
        //     return null;
        // }
    }


    [Serializable]
    public class RandomCoinsClass
    {
        //public Class RandomCoin;
    
        [SerializeField] private int coinWeight;
        [SerializeField] private GameObject coinprefab;
        [SerializeField] private int pointnumber;
    
        public int CoinWeight => coinWeight;
        public GameObject CoinPrefab => coinprefab;
        public int PointNumber => pointnumber;
    }
    
}