using UnityEngine;

namespace Runner
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "Configs/GameConfigs", order = 0)]
    public class GameConfigs : ScriptableObject
    {
        [SerializeField] private GameItemConfig[] items;

        public GameItemConfig GetItem()
        {
            var total = 0;
            foreach (var item in items)
            {
                total += item.Weight;
            }

            var rnd = Random.Range(0, total);
            var current = 0;
            foreach (var item in items)
            {
                current += item.Weight;
                if (rnd <= current)
                {
                    return item;
                }
            }

            return null;
        }
    }
}