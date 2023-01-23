using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Runner
{
    public class Level : MonoBehaviour
    {
        // ссылки на плеера и дорогу - перетаскиваем руками в инспекторе!
        [SerializeField] private GameObject roadPartPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject finishPrefab; // финишный кусок дороги
        [SerializeField] private GameObject wallPrefab; // стенка
        [SerializeField] private GameObject winparticles;
        //[SerializeField] private GameObject coinPrefab;

        // для расстановки стенок: между ними будет рандомиться расстановка стенок по ширине
        [SerializeField] private float minWallOffset = 3f;
        [SerializeField] private float maxWallOffset = 5f;

        // знать сколько кусокв хотим создать
        [SerializeField] private int roadPartCount = 10;

        // какая длина дороги
        // на случай если заготовим куски разной длины, чтобы ее использовать - брать ее из скейла
        [SerializeField] private float roadPartLength = 5;

        // ширина для расстановки стенок
        [SerializeField] private float roadPartWigth = 6f;


        // заводим локальную ссылку на плеера для GeneratePlayer
        private PlayerController _player; // опять похожих развели >:(
        private GameManager _gamemanagerfile;


        // и заинкапсулировать её для передачи далее - ###############################[ а куда?  ]
        public PlayerController Player => _player;


        private ParticleSystem _winparticles;

        // счетчик уровня
        public int currentlevel = 0;

        // ПОДКЛЮЧАЮ РАНДОМ МОНЕТОК
        private RandomCoins _randomCoinsFile;

        //private CoinsOfLevelConfig _coinsOfLevelConfigfile;
        // public event Action GenerateRandomCoin; 

        //----------------переменные для стен и монеток---------------------

        private float fulllength;
        private float currentLength;
        private float wallOffsetX;
        private float startPosX;

        //=============================================================================================================
        


        public void GenerateLevel()
        {
            // очистить старый уровень перед тем как генерить новый
            Clear();

            currentlevel++;

            // уровень состоит из фаз:
            GenerateRoad();
            GeneratePlayer();
            GenerateWallsCoins();
        }

        //-------------------------------------- дестрой всего --------------------------------------------------------
        private void Clear()
        {
            // надо перебрать все дочерние компонеты и задестроить
            for (int i = 0; i < transform.childCount; i++)
            {
                // дестроим не чайлды а их геймобжекты
                Destroy(transform.GetChild(i).GameObject());
            }

            // сбрасываем ссылку на плеера 
            _player = null;
        }


        //------------------------------------------ дорога ------------------------------------------------------------
        private void GenerateRoad()
        {
            // заводим стартовую позицию, проще всего с нулевой - вектор 0
            var roadLocalPosition = Vector3.zero; // 0.0.0

            for (int i = 0; i < roadPartCount; i++)
            {
                // var - это получаем ссылку на нашу часть дороги
                var road = Instantiate(roadPartPrefab, transform);

                // говорим стать частью уровня, так как создаем это внутри левела ######################### не догоняю
                // создали дорогу, поставили в локальную позицию, 
                road.transform.localPosition = roadLocalPosition; // 0.0.0

                // подняли значение позиции на то, какая у нас длина: 
                roadLocalPosition.z += roadPartLength; // = оно же + то что после рано ---- 0.0.5 потом 0.0.10 итд
            }

            // ставим финишный кусок
            Instantiate(finishPrefab, roadLocalPosition, Quaternion.identity, transform); //################ и тут он
            // finishPrefab - создаем префаб, его руками назначили в поле инспектора
            // transform - назначаем ему родителя 
            // roadLocalPosition - ставим его в это локальное место 
            // Quaternion.identity - нулевой поворот - все значения прямо


            roadLocalPosition.z = roadLocalPosition.z + 4.5f;
            Instantiate(winparticles, roadLocalPosition, Quaternion.identity, transform);
            _winparticles = FindObjectOfType<ParticleSystem>();
        }


        //------------------------------------------ игрок ------------------------------------------------------------
        private void GeneratePlayer()
        {
            // создать плеера, var - получить на него ссылку   ############################[ ЧТО ТАКОЕ ТРАНСОФРМ?????? ]
            // внимание, тут объект типа "GameObject"
            var player = Instantiate(playerPrefab, transform);

            // сразу создаем, куда его будем ставить: x 0,y 0, z - в середину первого куска (чтобы не на краешке стоял)!
            // процессора хреново делят. Лучше МЕНЯТЬ на + - *
            player.transform.localPosition = new Vector3(0, 0, roadPartLength * 0.5f);

            // из-за того что он тут объект типа "GameObject" - надо получить его компонент PlayerController
            _player = FindObjectOfType<PlayerController>();
            _player.Dobezal += WinSalute;
        }


        //----------------------------------------- стены И МОНЕТКИ ---------------------------------------------------
        private void GenerateWallsCoins()
        {
            //подключились к файлу рандома монет
            //_randomCoinsFile = FindObjectOfType<RandomCoins>();
            
            string cointext = null;
            
            // ---- расчёты----------надо высчитать всю длину трассы от начала до финиша
            fulllength = roadPartCount * roadPartLength; // 10 частей * 5 = 50

            // откуда начинаем ставить стенки - пропускаем два сегмента
            currentLength = roadPartLength * 2f; // (или просто 2) стартуем с позиции 10

            // ставить стены в три позиции: слева, середина, право
            // надо получить ширину 1/3 дороги
            wallOffsetX = roadPartWigth * 0.33333f; // 2 

            // позиции: -3 -1 1 ширина 2
            // ширина 6     ------ ыыыыы
            startPosX = -roadPartWigth * 0.5f; // -3
            //-----------------------------------------------------------------------------------

            // цикл повторяет построение уровня не зависимо от сегментов дороги
            // цикл работает, пока текущая дина меньше всей длины трассы
            while (currentLength < fulllength - 1) // while < 50 --- после 1го раза придет 14,3
            {
                //----------------------- длина -------------------------------------
                // заводим два значения вверху - минимальный и макс оффсет <----- это НА СКОЛЬКО СМЕСТИЛИСЬ
                // а тут - насколько смещаем вперед -рандом.между ( мимальным, и максимальным)
                // между стенками бужет рандомно от 3 до 5
                // сначала было так:
                var zOffset = minWallOffset + Random.Range(minWallOffset, maxWallOffset);
                // потом меняли, но тогда стен сильно много, фиг добежишь:
                // var zOffset = Random.Range(minWallOffset, maxWallOffset);    // 3f - 5f -> 4.3

                // к текущему значению прибаляем наш оффсет
                currentLength += zOffset; // += прибавить к себе же // 10 + 4.3f = 14.3 по Z !

                // длину надо заклепить. Clamp - ограничить в доапазоне (входной параметр/кого держим, от, до)
                // чтобы она не вышла после добавлений за пределы максимальной длины
                currentLength = Mathf.Clamp(currentLength, 0, fulllength); // not > 50

                //в итоге нашли длину на которую себя сместим


                //-------------------------- ширина ----------------------------------
                // ширина стены 2
                // последнее значение не попадает в выборку рандома, это инты, значит будет только 0/1/2
                var rndPositionX = Random.Range(0, 3); // 0-1-2

                //                       -3          1/3 дороги   0/1/2
                //    например           -3          2*0 = -3
                var wallPositionX = startPosX + wallOffsetX * rndPositionX; // -3 + 2 * (1/0/2) = -1 по X

                // создаем вектор, который мы отмодифицируем
                var localPosition = Vector3.zero; //  новая позиция 0,0,0
                localPosition.x = wallPositionX; // -1, 0, 0
                localPosition.z = currentLength; // -1, 0, 14.3

                
                
                // **************************** отключить стены для дебага *******************************************
                //  создаем стену 
                 Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);
                //  wallPrefab - какой префаб - его назначили ручками
                //  transform - назначаем ему родителя 
                //  roadLocalPosition - ставим его в это локальное место 
                //  Quaternion.identity - нулевой поворот - все значения прямо
                // **************************** отключить стены для дебага *******************************************

                //---------------------------монетки-------------------------------------------------------
                //----- рандом оставшихся двух позиций для монетки

                int rndCoinX = 0;

                for (int i = 0; i < 3; i++)
                {
                    rndCoinX = Random.Range(0, 3);
                    if (rndCoinX != rndPositionX)
                        break;
                }

                // позиция, использующая рандом с проверкой
                var coinPositionX = 1 + startPosX + (wallOffsetX * rndCoinX);

                //----------------------------------------
                // монетки позиция
                var localCoinPosition = Vector3.zero; //  новая позиция 0,0,0
                localCoinPosition.x = coinPositionX;
                localCoinPosition.z = currentLength;
                
                // ставим монетки
                // получаем рандомный префаб монетки
                //++++++++++++++++ NEW: РАНДОМ С ВЕСАМИ ++++++++++++++++++++++++++++++++++++++
                //----------- АЛЛАХ ЗНАЕТ, КАК ЭТО РАБОТАЕТ ---------------------------------
                // работaло: GameObject a = 6.GoGenerateCoin();
                // var cl = _randomCoinsFile.GoGenerateCoin();
                
                
                 _randomCoinsFile = FindObjectOfType<RandomCoins>();
                
                 CoinConfig cl = _randomCoinsFile.GetOneCoin();
                 GameObject a = cl.CoinPrefab; // префаб взятый из полученного экземпляра класса
                
                 cointext = cl.PointNumber.ToString();
                 GameObject thiscoin = Instantiate(a, localCoinPosition, Quaternion.identity, transform);
                
                 // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                 //  // надпись на монетке:
                 //
                 // как мне обратиться к свойству переданной сюда монетки, (вывести текст, например)?
                 var wheretotext = thiscoin.GetComponentInChildren<TextMeshProUGUI>();
                 wheretotext.text = cointext;
                
            }
        }

        
        //------------------------------------ салют на финише -------------------------------------------------------
        private void WinSalute()
        {
            Debug.Log("Салют пыщ пыщ");
            _winparticles.Play(); //  РАБОТАЕТ НАКОНЕЦ!
        }
        
        private void OnDestroy()
        {
            _player.Dobezal -= WinSalute;
        }
    }
}