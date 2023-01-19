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
         private CoinConfig _thiscoin;

        [SerializeField] private GameConfigsContainer container;
        
        
        //----- работало: поля для конфига-------------------------------------
        //
        // [SerializeField] private CoinsOfLevelConfig _coinsOfLevelConfigfile;
        public CoinConfig GetOneCoin()
        { 
            // ёёёёёёё мое оно прикрепилось друг за другом!!!
            _thiscoin = container.ChooseLevel().GoGetCoin();
            return _thiscoin;
        }
        //----------------------------------------------------------------------
        
    }


    //---------- это вроде не используется более? ---------
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