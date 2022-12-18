using System;
using System.Collections;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;

        [SerializeField] private float delay = 3f;
        
        private Level _level;
        private UiController _uiController;

        public event Action LevelWin;
        public event Action LevelFail;
        public event Action LevelRestart;

        private void Awake()
        {
            _uiController = FindObjectOfType<UiController>();
        }
        
        private void Start()
        {
            SubscribeUi();
        }

        private void OnDestroy()
        {
            UnsubscribeUi();
        }

        private void SubscribeUi()
        {
            _uiController.OnStartButtonClick += StartLevel;
            _uiController.OnNextButtonClick += StartLevel;
            _uiController.OnRestartButtonClick += RestartLevel;
        }
        
        private void UnsubscribeUi()
        {
            _uiController.OnStartButtonClick -= StartLevel;
            _uiController.OnNextButtonClick -= StartLevel;
            _uiController.OnRestartButtonClick -= RestartLevel;
        }

        private void StartLevel()
        {
            _level = Instantiate(levelPrefab, transform);
            _level.GenerateLevel();
            _level.Player.IsActive = true;
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
        }

        private void RestartLevel()
        {
            LevelRestart?.Invoke();
        }
        
        private void OnWin()
        {
            StartCoroutine(WinWithDelay());
            LevelWin?.Invoke();
        }
        
        private void OnDead()
        {
            StartCoroutine(FailWithDelay());
            LevelFail?.Invoke();
        }

        private IEnumerator WinWithDelay()
        {
            yield return ClearDelay();
            _level.Clear();
        }
        
        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay();
        }

        private IEnumerator ClearDelay()
        {
            _level.Player.OnWin -= OnWin;
            _level.Player.OnDead -= OnDead;
            yield return new WaitForSeconds(delay);
        }
    }
}