using UnityEngine;

//=========== этот типо содержит массив с ссылками на маленькие конфиги и метод рандома ://// ==========

namespace Runner
{
    [CreateAssetMenu(fileName = "CoinsOfLevelConfig", menuName = "Configs/CoinsOfLevelConfig", order = 0)]
    
    public class CoinsOfLevelConfig : ScriptableObject
    {
        [SerializeField] private CoinConfig[] itemshere;


        //------ получить одну монетку ------------------------
        public CoinConfig GoGetCoin()
        {
            CoinConfig thiscoin = ScriptableObject.CreateInstance<CoinConfig>();
            return GetCoin();
        }

        
        //------ public GameItemConfig GetCoin()
         CoinConfig GetCoin()
        {
            
            // первый прогон массива для получения суммы весов
            int total = 0;
            foreach (var item in itemshere)
            {
                total += item.CoinWeight; // 10, 40, 80
            } // по итогу 130
            
            // второй прогон массива с рандомом и сравнением 
            var rnd = Random.Range(0, total); // дает число от 0 до 130
            var current = 0;
            foreach (var item in itemshere) //для каждого типа монетки
            {
                //переменная current будет 10, 50, 120
                current = current + item.CoinWeight; //или  сurrent += item.CoinWeight

                // 55 <= 10,   89 <= 40,  7 <= 80    
                if (rnd <= current)
                {
                    return item;
                }
            }
            return null;
        }
    }
}