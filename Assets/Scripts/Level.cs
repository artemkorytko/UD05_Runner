using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private GameObject roadPartPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject finishPrefab;
        [SerializeField] private GameObject wallPrefab;

        [SerializeField] private int roadPartCount = 10;
        [SerializeField] private float roadPartLenght = 5f;
        [SerializeField] private float roadPartWidth = 6f;

        [SerializeField] private float minWallOffset = 3f;
        [SerializeField] private float maxWallOffset = 5f;

        private PlayerController _player;

        public PlayerController Player => _player;
        
        public void GenerateLevel()
        {
            Clear();
            GenerateRoad();
            GeneratePlayer();
            GenerateWalls();
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
                road.transform.localPosition = roadLocalPosition;
                roadLocalPosition.z += roadPartLenght;
            }

            Instantiate(finishPrefab, roadLocalPosition, Quaternion.identity, transform);
        }
       
        private void GeneratePlayer()
        {
            var player = Instantiate(playerPrefab, transform);
            playerPrefab.transform.localPosition = new Vector3(0f, 0f, roadPartLenght * 0.5f);

            _player = player.GetComponent<PlayerController>();
        }
        
        private void GenerateWalls()
        {
            var fullLenght = roadPartCount * roadPartLenght;
            var currentLenght = roadPartLenght * 2f;
            var wallOffsetX = roadPartWidth * 0.33333f;
            var startPosX = -roadPartWidth * 0.5f;

            while (currentLenght < fullLenght)
            {
                var zOffset = Random.Range(minWallOffset, maxWallOffset);
                currentLenght += zOffset;
                currentLenght = Mathf.Clamp(currentLenght, 0, fullLenght);

                var rndPositionX = Random.Range(0, 3);
                var wallPositionX = startPosX + wallOffsetX * rndPositionX;
                var localPosition = Vector3.zero;
                localPosition.x = wallOffsetX;
                localPosition.z = currentLenght;

                Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);
            }

        }
}   }