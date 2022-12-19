using System;
using System.Collections;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        private const string SAVE_LEVEL = "level_index";
        private const string SAVE_COIN = "Coints";
        
        [SerializeField] private Level _levelPrefab;
        [SerializeField] private float _delay = 1f;

        //private PlayerController _player;
        private UIController _uiController;
        private Level _level;
        
        private int _currentlevel;
        private int _coins;
        
        public event Action<int> OnAddCoin;
        public event Action Win;
        public event Action Fail;
        public event Action<int> OnNextLevelIndex;

      //  public PlayerController Player => _player;

        private int LevelIndex
        {
            get => _currentlevel;
            set
            {
                _currentlevel = value;
                OnNextLevelIndex?.Invoke(LevelIndex);
            }
        }
        
        private void Awake()
        {
            _uiController = FindObjectOfType<UIController>();
        }

        private void Start()
        {
            LoadData();
            _uiController.OnStartGame += StartGame;
            _uiController.OnRestartLevel += RestartCurrentLevel;
        }

        private void OnDestroy()
        {
            SaveData(SAVE_COIN, _coins);
            SaveData(SAVE_LEVEL, LevelIndex);
            
            _uiController.OnStartGame -= StartGame;
            _uiController.OnRestartLevel -= RestartCurrentLevel;
        }
        
        private void StartGame()
        {
            if (_level != null)
            {
                Destroy(_level.gameObject);
                LevelIndex++;
            }

            _level = Instantiate(_levelPrefab, transform);
            StartLevel();
        }

        private void StartLevel()
        {
            _level.GenegateLevel();
            _level.Player.IsActive = true; 
            //_player = _level.Player;
            
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
            _level.Player.OnCoint += AddCoint;
        }
        
        private void RestartCurrentLevel()
        {
            _level.RestartLevel();
            _level.Player.IsActive = true;
           // _player = _level.Player;
            
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
            _level.Player.OnCoint += AddCoint; 
        }

        private void AddCoint(int coin)
        {
            _coins++;
            OnAddCoin?.Invoke(_coins);
        }

        private void OnDead()
        {
            
            StartCoroutine(FailWithDelay());  // запуск карутины (карутины привязаны к компаненту, если я выключу GameManager, то карутира также откл)
            //StopCoroutine(); - останавливает карутину
            //StopAllCoroutines(); - остановить все карутины (данный случай в компаненте GameManager)
        }

        private void OnWin()
        {
            _level.ParticlePrefab.Play();
            StartCoroutine(WinWithDelay());
        }

        private IEnumerator WinWithDelay() // карутина это соПрограмма которая запустилась ПАРАЛЛЕЛЬНО нам!!!!!!!!!
        {
            yield return ClearDelay();
            Win?.Invoke(); 
        }


        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay(); // запуск карутины тип как StartCoroutine(ClearDelay());
            Fail?.Invoke();
        }

        private IEnumerator ClearDelay()
        {
            yield return new WaitForSeconds(_delay);
            
            _level.Player.OnWin -= OnWin;
            _level.Player.OnDead -= OnDead;
            _level.Player.OnCoint -= AddCoint;
        }
        
        private void SaveData(string key, int index)  
        {
            PlayerPrefs.SetInt(key, index);
        }

        private void LoadData() 
        {
            _currentlevel = PlayerPrefs.GetInt(SAVE_LEVEL, 0);
            _coins = PlayerPrefs.GetInt(SAVE_COIN, 0);
        }
    }
}