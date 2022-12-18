using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;
        [SerializeField] private int delay = 1;

        private Level _level;
        private Coroutine _coroutine;
        private UiController _uiController;
        

        private void Awake()
        {
            _uiController = FindObjectOfType<UiController>();
            _level = Instantiate(levelPrefab, transform);
        }

        private void Start()
        {
            SubscribeUI();
        }

        private void SubscribeUI()
        {
            _uiController.OnStartButtonEvent += StartLevel;
            _uiController.OnNextButtonEvent += StartLevel;
        }

        private void OnDestroy()
        {
            UnSubsribeUI();
        }

        private void UnSubsribeUI()
        {
            _uiController.OnStartButtonEvent -= StartLevel;
            _uiController.OnNextButtonEvent -= StartLevel;
        }

        private void StartLevel()
        {
            _level.GenerateLevel();
            _level.Player.IsActive = true;
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
        }

        private void OnDead()
        {
            StartCoroutine(FailWithDelay());
        }

        private void OnWin()
        {
            StartCoroutine(WinWithDelay());
        }

        private IEnumerator WinWithDelay()
        {
            OnWinn?.Invoke();
            yield return ClearDelay();
        }

        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay();
            OnFAill?.Invoke();
        }

        private IEnumerator ClearDelay()
        {
            _level.Player.OnWin -= OnWin;
            _level.Player.OnDead -= OnDead;
            yield return new WaitForSeconds(delay);
        }

        public event Action OnWinn;
        public event Action OnFAill;
    }
}