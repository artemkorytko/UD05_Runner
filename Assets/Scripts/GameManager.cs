using System;
using System.Collections;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        private const string SAVE_LEVEL = "level_index";
        private const string SAVE_COINT = "Coints";
        
        [SerializeField] private Level _levelPrefab;
        [SerializeField] private float _delay = 1f;
        
        private UIController _uiController;
        private Level _level;
        private int _currentlevel;
        private int _coints;
        
        public event Action<int> OnAddCoint;
        public event Action Win;
        public event Action Fail;

        
        public event Action<int> OnNextLevelIndex; 
        
        private int LevelIndex
        {
            get => _currentlevel;
            set
            {
                _currentlevel = value;
                OnNextLevelIndex?.Invoke(LevelIndex);
            }
        }

        public int Coints
        {
            get => _coints;
            set
            {
                _coints = value; 
                OnAddCoint?.Invoke(Coints);
            }
        }

        private void Awake()
        {
            LoadData();
            _uiController = FindObjectOfType<UIController>();
        }

        private void Start()
        {
            _uiController.OnStartGame += StartGame;
            _uiController.OnRestartLevel += RestartCurrentLevel;
        }

        private void OnDestroy()
        {
            SaveData(SAVE_COINT, Coints);
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
            _level.Player.IsActive = true; // тут нужно поправить тип ДЗ
            
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
            _level.Player.OnCoint += AddCoint;
        }
        
        private void RestartCurrentLevel()
        {
            _level.RestartLevel();
            _level.Player.IsActive = true;
            
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
            _level.Player.OnCoint += AddCoint; 
        }

        private void AddCoint(int coint)
        {
            Coints++;
        }

        private void OnDead()
        {
            StartCoroutine(FailWithDelay());  // запуск карутины (карутины привязаны к компаненту, если я выключу GameManager, то карутира также откл)
            //StopCoroutine(); - останавливает карутину
            //StopAllCoroutines(); - остановить все карутины (данный случай в компаненте GameManager)
        }

        private void OnWin()
        {
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
            _coints = PlayerPrefs.GetInt(SAVE_COINT, 0);
        }
    }
}