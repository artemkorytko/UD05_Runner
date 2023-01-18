using Runner.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class Level : MonoBehaviour
    {
         [SerializeField] private LevelConfig levelConfig;
        
         private GameObject _playerPrefab;
         private GameObject _finihsPrefab; 
         private GameObject _wallPrefab;
         private GameObject _roadPartPrefab;
         private GameObject _coinPrefab;
         private ParticleSystem _particlePrefab;
         
         private int _chanceDropCoin = 70;
         private int _roadPartCount = 10; 
         private float _roadPartLength = 6f;
         private float _roadPartWidth = 6f;
         private float _minWallsOffset = 3f;
         private float _maxWallsOffset = 5f;
         private float _distanceBetweenParticleAndFinish = 5f;
        
        private PlayerController _player;
        private ParticleSystem _particle;
        
        public PlayerController Player => _player; 
        public ParticleSystem ParticlePrefab => _particle;

        public void GenegateLevel()
        {
            SetLevelConfig();
            Clear();
            GenegateRoadAndParticle();
            GenegatePlayer();
            GenegateWallsAndCois();
        }

        private void SetLevelConfig()
        {
            _playerPrefab = levelConfig.PlayerPrefab;
            _finihsPrefab = levelConfig.FinihsPrefab;
            _coinPrefab = levelConfig.CoinPrefab;
            _wallPrefab = levelConfig.WallPrefab;
            _roadPartPrefab = levelConfig.RoadPartPrefab;
            _particlePrefab = levelConfig.ParticlePrefab;

            _chanceDropCoin = levelConfig.ChanceDropCoin;
            _roadPartCount = levelConfig.RoadPartCount;

            _roadPartLength = levelConfig.RoadPartLength;
            _roadPartWidth = levelConfig.RoadPartWidth;
            _minWallsOffset = levelConfig.MINWallsOffset;
            _maxWallsOffset = levelConfig.MAXWallsOffset;
            _distanceBetweenParticleAndFinish = levelConfig.DistanceBetweenParticleAndFinish;
        }

        private void Clear()
        {
            for (int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
            
            _player = null;
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
            particlePosition.y = -20f;
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

                
// шанс выпадания манетки 
                var randon = Random.Range(0, 100);
                if (randon <= _chanceDropCoin)
                {
                    var coinPositionX = startPosX + wallOffsetX * (randomPositionX != 0 ? randomPositionX != 2  ? 0 : 1 : 2);
                    var localPositionCoin = Vector3.zero;
                    localPositionCoin.z = currentLength;// + 2f;
                    localPositionCoin.x = coinPositionX;
                    Instantiate(_coinPrefab, localPositionCoin, Quaternion.identity, transform);
                }
                
                Instantiate(_wallPrefab, localPositionWall, Quaternion.identity, transform);
                
            }

        }

    }
}