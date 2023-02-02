using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner.Lesson21
{
    public class RandomManagerVesi : MonoBehaviour
    {
        [SerializeField] private GameConfigs items; // сюда назначается конфиг с которым мы будем работать (вся логика прописана там(тож самое что и ниже))

        private GameObject _currentObject;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Destroy(_currentObject);
            
            _currentObject = Instantiate(items.GetItem().Prefab); 
        }

        // private RandomItem GetItem()
        // {
        //     var total = 0;
        //     foreach (var item in items)
        //         total += item.Weigth;
        //     
        //     var random = Random.Range(0, total);
        //     var currentValue = 0;
        //     foreach (var item in items)
        //     {
        //         currentValue += item.Weigth;
        //         if (random <= currentValue)
        //             return item;
        //     }
        //     return null;
        // }
    }

    [System.Serializable]
    public class RandomItem
    {
        [SerializeField] private int weigth; // вес item это (чем больше тем и важнее значимость выпадения этого префаба)
        [SerializeField] private GameObject prefab;
        
        public int Weigth => weigth;
        public GameObject Prefab => prefab;
    }

}