using UnityEngine;

namespace Runner
{
    [CreateAssetMenu(fileName = "GameItemConfig", menuName = "Configs/GameItemConfig", order = 0)]
    public class GameItemConfig : ScriptableObject
    {
        [SerializeField] private int weight;
        [SerializeField] private GameObject prefab;

        public int Weight => weight;

        public GameObject Prefab => prefab;
    }
}