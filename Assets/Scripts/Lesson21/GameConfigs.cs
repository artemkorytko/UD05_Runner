using UnityEngine;

namespace Runner.Lesson21
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Configs/GameConfigs", order = 0)]
    public class GameConfigs : ScriptableObject
    {
        [SerializeField] private GameItemConfig[] items;
        
        public GameItemConfig GetItem()
        {
            var total = 0;
            foreach (var item in items)
                total += item.Weigth;
            
            var random = Random.Range(0, total);
            var currentValue = 0;
            foreach (var item in items)
            {
                currentValue += item.Weigth;
                if (random <= currentValue)
                    return item;
            }
            return null;
        }
    }
}