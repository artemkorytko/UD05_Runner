using UnityEngine;

//-------- первый эксперементальный конфиг (Less 21, 2:11)-----------
namespace Runner
{
    // FILENAME - АК предпочитает шоб соответствовал классу
    // MENUNAME - классу, который стоит в конфигах
    [CreateAssetMenu(fileName = "CoinConfig", menuName = "Configs/CoinConfig", order = 0)]
    
    // внимание, наследуемся не от монобеха, а от скриптабл!
    // это встроенный класс-контейнер для работы с информацией, поддерживает сценарии
    public class CoinConfig : ScriptableObject
    {
        // имеет все те поля, что у бывшего Coin CLass.
        [SerializeField] private int coinWeight;
        [SerializeField] private GameObject coinprefab;
        [SerializeField] private int pointnumber;

        public int CoinWeight => coinWeight;
        public GameObject CoinPrefab => coinprefab;
        public int PointNumber => pointnumber;
    }
}