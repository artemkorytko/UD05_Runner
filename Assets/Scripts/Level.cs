using UnityEngine;

namespace Runner_
{
    public class Level : MonoBehaviour //level отвечает за генерацию уровня
    {
        
        [SerializeField] private GameObject roadPartPrefab;
       [SerializeField] private GameObject playerPrefab;
       [SerializeField] private GameObject finishPrefab;
       [SerializeField] private GameObject wallPrefab;
       

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
           GenerateWalls();
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
       
       private void GenerateWalls()
       {
           var fullLength = roadPartCount * roadPartLength; //длина всей дороги
           var currentLength = roadPartLength * 2; //откуда начнется генерация рпепятствий 
           var wallOffsetX = roadPartWidth * 0.33333f;
           var startPosX = - roadPartWidth * 0.5f;

           while (currentLength < fullLength)
           {
               var zOffset = Random.Range(minWallOffset, maxWallOffset);
               currentLength += zOffset;
               currentLength = Mathf.Clamp(currentLength, 0, fullLength); // длина позиции на которую себя сместим

               var rndPositionX = Random.Range(0, 3); // либо 0 либо 1 либо 2  (последнее т е 3 не будет в выборке при рандоме)
               var wallPositionX = startPosX + wallOffsetX * rndPositionX;

               var localPosition = Vector3.zero;
               localPosition.x = wallPositionX;
               localPosition.z = currentLength;

               Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);
           }

       }

    }
}