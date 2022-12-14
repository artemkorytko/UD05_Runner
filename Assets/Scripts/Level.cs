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

            Instantiate(finishPrefab, roadLocalPosition, Quaternion.identity,transform); // созд префаб, назнач родителя и ставим в место (roadLocalPosition) и ставим поворот (quaternion)
        }
        
        private void GeneratePlayer()
        {
           var player = Instantiate(playerPrefab, transform); 
           playerPrefab.transform.localPosition = new Vector3(0f, 0f, roadPartLength * 0.5f);

           _player = player.GetComponent<PlayerController>();
        }
        
        private void GenerateWalls()
        {
            var fullLength = roadPartCount * roadPartLength;
            var currentLength = roadPartLength * 2f;
            var wallOffsetX = roadPartWidth * 0.333f;
            var startPosX = -roadPartWidth * 0.5f;

            while (currentLength < fullLength)
            {
                var zOfsset = minWallOffset + Random.Range(minWallOffset, maxWallOffset);
                currentLength += zOfsset;
                currentLength = Mathf.Clamp(currentLength, 0, fullLength);

                var RandomPositionX = Random.Range(0, 3);
                var wallPositionX = startPosX + wallOffsetX * RandomPositionX;
                var localPosition = Vector3.zero;
                localPosition.x = wallPositionX;
                localPosition.z = currentLength;

                Instantiate(wallPrefab, localPosition, Quaternion.identity, transform);
            }

        }
    }
}