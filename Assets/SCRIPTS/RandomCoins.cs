using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


//--------------- висит на пустом объекте, даем ему ссылку на контейнер конфигов -------------
namespace Runner
{
    public class RandomCoins : MonoBehaviour
    {
         private CoinConfig _thiscoin;

        [SerializeField] private GameConfigsContainer container;
        
        
        //----- в эту функцию лазит Level, когда ставит монетки ----------------------------------------
        public CoinConfig GetOneCoin()
        { 
            // ёёёёёёё мое оно прикрепилось друг за другом!!!
            _thiscoin = container.ChooseLevel().GoGetCoin();
            return _thiscoin;
            // идет в контейнер конфигоа, там выбирает левел относительно того, что в GameManagere за тип уровня, 
            // и в нем уже выбирает мометку согласно добавленным в конфиг
        }
        //------------------------------------------------------------------------------------------------
    }
    
}