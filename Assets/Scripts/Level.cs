using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runner
{
    public class Level : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _finihsPrefab;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _roadPartPrefab;
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private ParticleSystem _particlePrefab;

        [Header("Settings Road")] 
        [SerializeField] private int _roadPartCount = 10;
        [SerializeField] private float _roadPartLength = 6f;
        [SerializeField] private float _roadPartWidth = 6f;

        [Header("Settings Walls")] 
        [SerializeField]private float _minWallsOffset = 3f;
        [SerializeField] private float _maxWallsOffset = 5f;

        [Header(("Particle System"))] [SerializeField]
        private float _distanceBetweenParticleAndFinish = 5f;
        
        private PlayerController _player;
        private ParticleSystem _particle;
        
        public PlayerController Player => _player; // ссылка на игрока
        public ParticleSystem ParticlePrefab => _particle;

        public void GenegateLevel()
        {
            Clear();
            GenegateRoadAndParticle();
            GenegatePlayer();
            GenegateWallsAndCois();
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

        private void GenegateRoadAndParticle()
        {
            var roadLocalPosition = Vector3.zero;
            for (int i = 0; i < _roadPartCount; i++)
            {
                var road = Instantiate(_roadPartPrefab, transform);
                road.transform.localPosition = roadLocalPosition;
                roadLocalPosition.z += _roadPartLength;
            }
            Instantiate(_finihsPrefab, roadLocalPosition, Quaternion.identity, transform);

            // тут партикал
            var particlePosition = Vector3.zero;
            particlePosition.z = roadLocalPosition.z + _distanceBetweenParticleAndFinish;
            
            _particle = Instantiate(_particlePrefab, particlePosition, Quaternion.Euler(-90,0,0), transform);
            _particle.Stop();
          
        }

        private void GenegatePlayer()
        {
            var player = Instantiate(_playerPrefab, transform);
            player.transform.localPosition = new Vector3(0f, 0f, _roadPartLength * 0.5f); //  * 0.5f - стараемся всегда заменять деление(/) на умножение(*) !!!

            _player = player.GetComponent<PlayerController>();
        }


        private void GenegateWallsAndCois()
        {
            var fullLength = _roadPartCount * _roadPartLength;
            var currentLength = _roadPartLength * 2f;
            var wallOffsetX = _roadPartWidth * 0.33333f;
            var startPosX = -_roadPartWidth * 0.5f;

            while (currentLength < fullLength)
            {
                //расчет по Z(т.е определение длины расстояния между Wall)            
                var zOffset = Random.Range(_minWallsOffset, _maxWallsOffset); // растояние жеду wall
                currentLength += zOffset;
                currentLength =
                    Mathf.Clamp(currentLength, 0,
                        fullLength); //(если сделать fullLength - 1 юнити "ГГ" ) // Mathf.Clamp(currentLength не может быть меньше 0, и не смотжет быть больше fullLength)

                // расчетпо X (т.е тут происходит определение где установить стену (0, 1, 2))
                var randomPositionX = Random.Range(0, 3); // 3 не учитывается в Range!!!!
                var wallPositionX = startPosX + wallOffsetX * randomPositionX;

                // просто запись Vector3 и присвоение значений осей X and Z (что вычислялись выше)!!!
                var localPositionWall = Vector3.zero;
                localPositionWall.x = wallPositionX;
                localPositionWall.z = currentLength;

                // для монет по X and Z
                var randomPositionCoinX = Random.Range(0, 3);
                var coinPositionX = startPosX + wallOffsetX * randomPositionCoinX;

                var localPositionCoin = Vector3.zero;
                localPositionCoin.z = currentLength + 2f;
                localPositionCoin.x = coinPositionX;

                Instantiate(_wallPrefab, localPositionWall, Quaternion.identity, transform);
                Instantiate(_coinPrefab, localPositionCoin, Quaternion.identity, transform);
            }

        }

    }
}