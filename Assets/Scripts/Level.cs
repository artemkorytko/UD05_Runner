using UnityEngine;

namespace Runner_
{
    public class Level : MonoBehaviour //level отвечает за генерацию уровня
    {
        
        [SerializeField] private GameObject roadPartPrefab;
       [SerializeField] private GameObject playerPrefab;
       [SerializeField] private GameObject finishPrefab;
       [SerializeField] private GameObject wallPrefab;
       [SerializeField] private GameObject coinPrefab;
       

       [SerializeField] private int roadPartCount = 10;
       [SerializeField] private float roadPartLength = 5f;
       [SerializeField] private float roadPartWidth = 6f;

       [SerializeField] private float minWallOffset = 3f;
       [SerializeField] private float maxWallOffset = 5f;
       
       private PlayerController _player;

       public PlayerController Player => _player; // заинкапсулировали
       // private void Start()
       // {
       //     GenerateLevel();
       // }

       public void GenerateLevel()
       {
           Clear(); //делаем очистку перед тем как пойти дальше чтобы при генераци например следующего уровня у нас не существовало два уровня одновременно
           GenerateRoad();
           GeneratePlayer();
           GenerateWallsAndCoins();
       }

       public void ContinueLevelAfterFail() //заходим в этот метод при рестарте уровня(т е локация сохраняется старого уровня, а персонаж создается заново в Нулевых позициях)
       {
           Destroy(_player.gameObject); //(для этого сначала удаляем старогог плеера)
           GeneratePlayer(); // а потом генерим плеера
       }
       

       private void Clear()
       {
           for (int i = 0; i < transform.childCount; i++)
           {
               Destroy(transform.GetChild(i).gameObject);
           }

           _player = null;
       }


       private void GenerateRoad()
       {
           var roadLocalPosition = Vector3.zero;

           for (int i = 0; i < roadPartCount; i++)
           {
               var road = Instantiate(roadPartPrefab, transform);
               road.transform.localPosition = roadLocalPosition; //0.0.0 //0.0.5
               roadLocalPosition.z += roadPartLength; //0.0.5 //0.0.10
           }

           Instantiate(finishPrefab, roadLocalPosition, Quaternion.identity, transform);//Quaternion.identity - нулевой кватерион (все позиции 0, 0, 0, 0)
       }
       
       private void GeneratePlayer()
       {
           var player = Instantiate(playerPrefab, transform);
           player.transform.localPosition = new Vector3(0f, 0f, roadPartLength * 0.5f);
           
           _player = player.GetComponent<PlayerController>();
       }
       
       private void GenerateWallsAndCoins()
       {
           var fullLength = roadPartCount * roadPartLength; //длина всей дороги
           var currentLength = roadPartLength * 2; //откуда начнется генерация рпепятствий 
           var wallOffsetX = roadPartWidth * 0.33333f;/* 2 юнита
           разбиваем ширину дороги на 3 чтобы в дальнейшем указать (например при ширине дорогри 5 метров) что 
           начало стены может быть в коориданте 0 или в координате 1,7 или в координате 3,4 м */
           var startPosX = - roadPartWidth * 0.5f; //startPosX

           while (currentLength < fullLength) //генерим стену
           {
               var zOffset = Random.Range(minWallOffset, maxWallOffset);
               currentLength += zOffset; //следующее препятствие будет дальше от предыдущего минимум на 3 максимум на 5 значений
               currentLength = Mathf.Clamp(currentLength, 0, fullLength); // длина позиции на которую себя сместим

               var rndPositionX = Random.Range(0, 3); // либо 0 либо 1 либо 2  (последнее т е 3 не будет в выборке при рандоме)
               var wallPositionX = startPosX + wallOffsetX * rndPositionX; /*-3+2*0 =-3// -1 // 1 ( то есть по ширине начало стены
                может быть в координате -3 относительно пола, либо в координате -1 относительно пола либо в координате 1 относиетльно пола
               */

               var localPosition = Vector3.zero; /*создали пустой трехкмерный вектор в который дальше будет засовывать координаты 
               сгенеренной стены (в созданном трехмерном векторе изначально все координаты 0) */
               localPosition.x = wallPositionX; //в координату Х пустого трехмерного вектор засовываем координату Х сгенеренной стены
               localPosition.z = currentLength;//в координату Z пустого трехмерного вектор засовываем координату Z сгенеренной стены
               // а координата У всегда 0

               Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);/*создаем сгенеренную стену в которую помещаем сам 
                объект(префаб) стену, ее позиции и повороты, трансформ - значит что стена будет дочерним объектом какого-либо друого 
                объекта (в нашем случае: стена - дочерний объект Левела)*/

               var RndCoinPositionX = Random.Range(0, 7);
               var CoinPositionX = startPosX + RndCoinPositionX;
               var CoinPositionZ = currentLength + 1.5f;
               var localCoinPosition = Vector3.zero;
               localCoinPosition.x = CoinPositionX;
               localCoinPosition.z = CoinPositionZ;
               localCoinPosition.y = 1.5f;
               Instantiate(coinPrefab, localCoinPosition, Quaternion.identity, transform);
           }

       }

    }
}