using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class RandomManager : MonoBehaviour
    {
        [SerializeField] private GameConfigsContainer item;

        private GameObject _currentObject;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_currentObject)
                    Destroy(_currentObject);
                _currentObject = Instantiate(item.GetConfig(ConfigType.Easy).GetItem().Prefab);
            }
        }

        // private RandomItem GetItem()
        // {
        //     var total = 0;
        //     foreach (var item in items) //5, 20, 50, 80, 100 == 255
        //     {
        //         total += item.Weight;
        //     } // подсчитали общий вес элементов
        //
        //     var rnd = Random.Range(0, total); // получили рандомный вес (0: 255)
        //     var current = 0;
        //     foreach (var item in items) //ищем подходящий
        //     {
        //         // 58
        //         current += item.Weight; // 58 < 5 == false 58 < 28 == false 78 < 58  == true
        //         if (rnd <= current)
        //         {
        //             return item;
        //         }
        //     }
        //
        //     return null;
        // }
    }

    [Serializable]
    public class RandomItem
    {
        [SerializeField] private int weight;
        [SerializeField] private GameObject prefab;

        public int Weight => weight;

        public GameObject Prefab => prefab;
    }
}