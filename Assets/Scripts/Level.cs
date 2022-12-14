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
        [SerializeField] private float roadPartLength = 5f;
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
                roadLocalPosition.z += roadPartLength;
            }

            Instantiate(finishPrefab, roadLocalPosition, Quaternion.identity, transform);
        }

        private void GeneratePlayer()
        {
            var player = Instantiate(playerPrefab, transform);
            playerPrefab.transform.localPosition = new Vector3(0f, 0f, roadPartLength * 0.5f);

            _player = player.GetComponent<PlayerController>();
        }

        private void GenerateWalls()
        {
            var fullLenght = roadPartCount * roadPartLength; //50 all
            var currentLength = roadPartLength * 2f; //10 start
            var wallOffsetX = roadPartWidth * 0.33333f; //2 
            var startPosX = -roadPartWidth * 0.5f; //-3

            while (currentLength < fullLenght) //while < 50
            {
                var zOffset = Random.Range(minWallOffset, maxWallOffset); // 3f - 5f-> 4.3f
                currentLength += zOffset; // 10 + 4.3f = 14.3 //pos z
                currentLength = Mathf.Clamp(currentLength, 0, fullLenght); //not > 50

                var rndPositionX = Random.Range(0, 3); //0, 1, 2
                var wallPositionX = startPosX + wallOffsetX * rndPositionX; // -3 + 2 * (2) = 1 // pos x
                var localPosition = Vector3.zero; //init new pos (0.0.0)
                localPosition.x = wallPositionX; // -1.0.0
                localPosition.z = currentLength; // -1.0.14,3

                Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);
            }
        }
    }
}