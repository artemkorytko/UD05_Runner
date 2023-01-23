using System;
using System.Data.Common;
using UnityEngine;

//------------------- КОНТЕЙНЕР КОНФИГОВ ---------------------------
//------- типо тут например выбираем уровень по логике -------------
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
            //_gotleveltype = _gameManagerfile.leveltype;
            howmanyLevelConfigs = configsarray.Length;
        }


        //----------- пытаюсь что-то добавить
        //----------- на входе прикрутим в GameManager счетчик сколько пробежал без ошибки ------
        public CoinsOfLevelConfig ChooseLevel()
        {
            _gameManagerfile = FindObjectOfType<GameManager>();
            _gotleveltype = _gameManagerfile.leveltype;

            // меняет уровень в зависимости от типа уровня из GameManager
            // то бишь сколько раз без ошибок пробежал
            switch (_gotleveltype)
            {
                case 2: return GetLevel2Coins(); // брейки не надо ибо есть ретурн


                case 1: return GetLevel1Coins();


                default: return GetLevel0Coins();
            }
        }
        //-------------------------------

        // возвращает конфиг левела кудааа - в гейм менеджер
        public CoinsOfLevelConfig GetLevel0Coins() // (ConfigType type)
        {
            // по какой-то логике ретурнаем тимп конфига
            _levelchosen = configsarray[0];
            return _levelchosen;
        }

        public CoinsOfLevelConfig GetLevel1Coins() // (ConfigType type)
        {
            // по какой-то логике ретурнаем тимп конфига
            _levelchosen = configsarray[1];
            return _levelchosen;
        }

        public CoinsOfLevelConfig GetLevel2Coins() // (ConfigType type)
        {
            // по какой-то логике ретурнаем тимп конфига
            _levelchosen = configsarray[2];
            return _levelchosen;
        }
        // тут некрасиво, что пока три левела, а потом чего?????????????
    }

    // типо список названий
    public enum ConfigType
    {
        Easy,
        Mid,
        Hard,
        Insane
    }
}