using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class Level : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _finihsPrefab;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _roadPartPrefab;
        [SerializeField] private GameObject _cointPrefab;
        
        [Header("Settings Road")]
        [SerializeField] private int _roadPartCount = 10;
        [SerializeField] private float _roadPartLength = 5f;
        [SerializeField] private float _roadPartWidth = 6f;

        [Header("Settings Walls")]
        [SerializeField] private float _minWallsOffset = 3f;
        [SerializeField] private float _maxWallsOffset = 5f;

        private PlayerController _player;
        
        public PlayerController Player => _player; // ссылка на игрока
        
        public void GenegateLevel()
        {
            Clear();
            GenegateRoad();
            GenegatePlayer();
            GenegateWallsAndCoits();
        }

        private void Clear()
        {
            for (int i = 0; i < transform.childCount; i++) 
            {
                Destroy(transform.GetChild(i).gameObject); // удаление всех дочерних объектов ()
            }
            
            _player = null; // сброс ссылки плейера
        }

        public void RestartLevel()
        {
            Destroy(_player.gameObject);
            GenegatePlayer();
        }

        private void GenegateRoad()
        {
            var roadLocalPosition = Vector3.zero;
            for (int i = 0; i < _roadPartCount; i++)
            {
               var road = Instantiate(_roadPartPrefab, transform);
               road.transform.localPosition = roadLocalPosition;
               roadLocalPosition.z += _roadPartLength;
            }
            
            Instantiate(_finihsPrefab, roadLocalPosition, Quaternion.identity, transform);
        }
        
        private void GenegatePlayer() 
        {
            var player = Instantiate(_playerPrefab, transform);
            player.transform.localPosition = new Vector3(0f,0f, _roadPartLength * 0.5f); //  * 0.5f - стараемся всегда заменять деление(/) на умножение(*) !!!

            _player = player.GetComponent<PlayerController>();
        }
        
        
        private void GenegateWallsAndCoits()
        {
            var fullLength = _roadPartCount * _roadPartLength;
            var currentLength = _roadPartLength * 2f;
            var wallOffsetX = _roadPartWidth * 0.33333f; 
            var startPosX = - _roadPartWidth * 0.5f;

            while (currentLength < fullLength)
            {
                //расчет по Z(т.е определение длины расстояния между Wall)            
                var zOffset = Random.Range(_minWallsOffset, _maxWallsOffset); // растояние жеду wall
                currentLength += zOffset;
                currentLength = Mathf.Clamp(currentLength, 0, fullLength);//(если сделать fullLength - 1 юнити "ГГ" ) // Mathf.Clamp(currentLength не может быть меньше 0, и не смотжет быть больше fullLength)

                // расчетпо X (т.е тут происходит определение где установить стену (0, 1, 2))
                var randomPositionX = Random.Range(0, 3);  // 3 не учитывается в Range!!!!
                var wallPositionX = startPosX + wallOffsetX * randomPositionX;

                // просто запись Vector3 и присвоение значений осей X and Z (что вычислялись выше)!!!
                var localPositionWall = Vector3.zero;
                localPositionWall.x = wallPositionX;
                localPositionWall.z = currentLength;
                
                // для монет по X and Z
                var randomPositionCointX = Random.Range(0, 3);
                var cointPositionX = startPosX + wallOffsetX * randomPositionCointX;
                
                var localPositionCoint = Vector3.zero;
                localPositionCoint.z = currentLength + 2f; 
                localPositionCoint.x = cointPositionX;
                
                Instantiate(_wallPrefab, localPositionWall, Quaternion.identity, transform);
                Instantiate(_cointPrefab, localPositionCoint, Quaternion.identity, transform);
            }

        }

    }
}