using UnityEngine;

//------------------- КОНТЕЙНЕР КОНФИГОВ ---------------------------
//------- типо тут например выбираем уровень по логике -------------
//------- в уровне кранится, какие монетки появляются --------------
namespace Runner
{
    [CreateAssetMenu(fileName = "GameConfigsContainer", menuName = "Configs/GameConfigsContainer", order = 0)]
    public class GameConfigsContainer : ScriptableObject
    {
        [SerializeField] private CoinsOfLevelConfig[] coinsOfLevelConfig;

        private CoinsOfLevelConfig _thislevelcoins;
        
        // возвращает конфиг левела
        public CoinsOfLevelConfig GetCoinsOfLevelConfig()// (ConfigType type)
        {
            // по какой-то логике ретурнаем тимп конфига
            _thislevelcoins =  coinsOfLevelConfig[0];
            return _thislevelcoins;
        }


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