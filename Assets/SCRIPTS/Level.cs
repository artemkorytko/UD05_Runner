using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{


    public class Level : MonoBehaviour
    {
        // ссылки на плеера и дорогу - перетаскиваем руками в инспекторе!
        [SerializeField] private GameObject roadPartPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject finishPrefab; // финишный кусок дороги
        [SerializeField] private GameObject wallPrefab; // стенка

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
        
        //на старте генерируем уровень, массивом создаем дорогу и плеера на ней
        void Start()
        {
            GenerateLevel(); // пока дебажно тут, потом надо засунуть в гейм менеджер
        }

        
        
        public void GenerateLevel()
        {
            //уровень состоит из фаз:
            // создаем дорогу
            GenerateRoad();
            GeneratePlayer();
            GeneratWalls();
        }

        


        private void GenerateRoad()
        {
            // заводим стартовую позицию, проще всего с нулевой - вектор 0
            var roadLocalPosition = Vector3.zero; // 0.0.0

            for (int i = 0; i < roadPartCount; i++)
            {
                // var - это получаем ссылку на нашу часть дороги
                var road = Instantiate(roadPartPrefab, transform);
                
                // говорим стать частью уровня, так как создаем это внутри левела
                // создали дорогу, поставили в локальную позицию, 
                road.transform.localPosition = roadLocalPosition; // 0.0.0
                
                // подняли значение позиции на то, какая у нас длина: 
                roadLocalPosition.z += roadPartLength; // = оно же + то что после рано ---- 0.0.5 потом 0.0.10 итд
            }
            
            // ставим финишный кусок
            Instantiate(finishPrefab, roadLocalPosition, Quaternion.identity, transform);
            // finishPrefab - создаем префаб, его руками назначили в поле инспектора
            // transform - назначаем ему родителя 
            // roadLocalPosition - ставим его в это локальное место 
            // Quaternion.identity - нулевой поворот - все значения прямо
            
        }
        private void GeneratePlayer()
        {
            // создать плеера, var - получить на него ссылку, ЧТО ТАКОЕ ТРАНСОФРМ?????? <--------------------------
            // внимание, тут объект типа "GameObject"
            var player = Instantiate(playerPrefab, transform);
            
            // сразу создаем, куда его будем ставить: x 0,y 0, z - в середину первого куска (чтобы не на краешке стоял)!
            // процессора хреново делят. Лучше МЕНЯТЬ на + - *
            player.transform.localPosition = new Vector3(0, 0, roadPartLength * 0.5f);
            
            // из-за того что он тут объект типа "GameObject" - надо получить его компонент PlayerController
            _player = player.GetComponent<PlayerController>();
            
        }
        
        private void GeneratWalls()
                {
                    // надо высчитать всю длину трассы от начала до финиша
                    var fulllength = roadPartCount * roadPartLength;
                    
                    // откуда начинаем стаить стенки - пропускаем два сегмента
                    var currentLength = roadPartLength * 2f; //или просто 2
                    
                    // ставить стены в три позиции: слева, середина, право
                    // надо получить ширину 1/3 дороги
                    var wallOffsetX = roadPartWigth * 0.33333f;
                    
                    // позиции: -3 -1 1 ширина 2
                    // ширина 6     ------ ыыыыы
                    var startPosX = -roadPartWigth * 0.5f;
                    
                    //-----------------------------------------------------------------------------------
                    // цикл повторяет построение вровня не зависимо от сегментов дороги
                    // цикл работает, пока текущая дина меньше всей длины трассы
                    while (currentLength < fulllength)
                    {
                        //----------------------- длина -------------------------------------
                        // заводим два значения вверху - минимальный и макс оффсет <------ че такое оффсет?
                        // а тут - насколько смещаем вперед -рандом.между ( мимальным, и максимальным)
                        var zOffset = minWallOffset + Random.Range(minWallOffset, maxWallOffset);
                        
                        // к текущему значению прибаляем наш оффсет
                        currentLength += zOffset; // += прибавить к себе же
                        
                        // длину надо заклепить. Clamp - ограничить в доапазоне (входной параметр/кого держим, от, до)
                        // чтобы она не вышла после добавлений за пределы максимальной длины
                        currentLength = Mathf.Clamp(currentLength, 0, fulllength);

                        //в итоге нашли длину на которую себя сместим
                        
                        
                        //-------------------------- ширина ----------------------------------
                        // последнее значение не попадает в выборку рандома, это инты, значит будет только 0/1/2
                        var rndPositionX = Random.Range(0, 3); 
                        
                        //                       -3          1/3 дороги   0/1/2
                        //    например           -3          2*0 = -3
                        var wallPositionX = startPosX + wallOffsetX * rndPositionX;
                        
                        // создаем вектор, который мы отмодифицируем
                        var localPosition = Vector3.zero;
                        localPosition.x = wallPositionX;
                        localPosition.z = currentLength;
                        
                        // создаем стену
                        Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);
                        // wallPrefab - какой префаб - его назначили ручками
                        // transform - назначаем ему родителя 
                        // roadLocalPosition - ставим его в это локальное место 
                        // Quaternion.identity - нулевой поворот - все значения прямо
                        
                        // !!! ПЕРЕПИСАТЬ КАК У АК
                        
                        

                    }


                }
        
        void Update()
        {

        }
    }

}