using UnityEngine;

namespace Runner.Lesson21
{
    [CreateAssetMenu(fileName = "GameItemConfig", menuName = "Configs/GameItemConfig", order = 0)]
    public class GameItemConfig : ScriptableObject
    {
        [SerializeField] private int weigth; // вес item это (чем больше тем и важнее значимость выпадения этого префаба)
        [SerializeField] private GameObject prefab;
        
        public int Weigth => weigth;
        public GameObject Prefab => prefab;
    }
}