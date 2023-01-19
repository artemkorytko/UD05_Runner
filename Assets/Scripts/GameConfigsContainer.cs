using UnityEngine;

namespace Runner
{
    [CreateAssetMenu(fileName = "GameConfigsContainer", menuName = "Configs/GameConfigsContainer", order = 0)]
    public class GameConfigsContainer : ScriptableObject
    {
        [SerializeField] private GameConfigs[] configs;

        public GameConfigs GetConfig(ConfigType type)
        {
            //logic
            return configs[0];
        }
    }

    public enum ConfigType
    {
        None,
        Easy,
        Mid,
        Hard,
        ExtraHard
    }
    
}