using System;
using System.Data.Common;
using UnityEngine;

//------------------- КОНТЕЙНЕР КОНФИГОВ ---------------------------
//-------    тут например выбираем уровень по логике   -------------
//------- в уровне кранится, какие монетки появляются --------------
namespace Runner
{
    [CreateAssetMenu(fileName = "GameConfigsContainer", menuName = "Configs/GameConfigsContainer", order = 0)]
    public class GameConfigsContainer : ScriptableObject
    {
        [SerializeField] public CoinsOfLevelConfig[] configsarray;
        [HideInInspector] public int howmanyLevelConfigs;
        
        //-------общие настраиваемые переменные -----------------
        [SerializeField] public float coinrotSpeed = 1f;
        
        //----------------------------------
        private GameManager _gameManagerfile;
        private int _gotleveltype;

        private CoinsOfLevelConfig _levelchosen;
        
        private void Awake()
        {
            howmanyLevelConfigs = configsarray.Length;
        }
        
        // идет в GameManager, получает тип уровня (если пробежал без ошибок - тип увеличивается)
        // 1й тип - одни синие, 2 - цветные, 3 - только золотые монетки
        public CoinsOfLevelConfig ChooseLevel()
        {
            _gameManagerfile = FindObjectOfType<GameManager>();
            _gotleveltype = _gameManagerfile.leveltype;
            
            
            // как это изящней написать если левелов больше будет, массивом??????????????
            switch (_gotleveltype)
            {
                case 2: return GetLevel2Coins(); // брейки не надо ибо есть ретурн

                case 1: return GetLevel1Coins();
                
                default: return GetLevel0Coins();
            }
        }
        //--------------------------------------------------------------------------------------

        // возвращает конфиг левела - в гейм менеджер
        public CoinsOfLevelConfig GetLevel0Coins() // (ConfigType type)
        {
            _levelchosen = configsarray[0];
            return _levelchosen;
        }

        public CoinsOfLevelConfig GetLevel1Coins() // (ConfigType type)
        {
            _levelchosen = configsarray[1];
            return _levelchosen;
        }

        public CoinsOfLevelConfig GetLevel2Coins() // (ConfigType type)
        {
            _levelchosen = configsarray[2];
            return _levelchosen;
        }
        
    }

    // типо список названий - увы не поняла, как это использовать, 
    // ????????????????????????????????
    public enum ConfigType
    {
        Easy,
        Mid,
        Hard,
        Insane
    }
}