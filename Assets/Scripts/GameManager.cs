using System;
using System.Collections;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;
        [SerializeField] private float delay = 1f;
        private Level _level;

        private Coroutine _coroutine;

        private void Awake()
        {
            _level = Instantiate(levelPrefab, transform);
        }

        private void Start()
        {
            StartLevel();
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
            yield return ClearDelay();
            StartLevel();
            
            // yield return null;
            // yield return new WaitForEndOfFrame();
            // yield return new WaitForFixedUpdate();
            // yield return new WaitWhile(() => delay == 1);
            // yield return new WaitUntil(() => delay != 1);
            // yield return StartCoroutine(FailWithDelay());
        }

        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay();
            StartLevel();
        }

        private IEnumerator ClearDelay()
        {
            _level.Player.OnWin -= OnWin;
            _level.Player.OnDead -= OnDead;
            yield return new WaitForSeconds(delay);
        }
    }
}