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

        private Level _level;

        private Coroutine _coroutine; // это переменная будет хранить ссылку на карутину (для успешной остановки карутины когда их дохрена...)

        private void Awake()
        {
            _level = Instantiate(_levelPrefab, transform);
        }

        private void Start()
        {
            StartLevel();
        }

        private void StartLevel()
        {
            _level.GenegateLevel();
            _level.Player.IsActive = true; // тут нуно поправить тип ДЗ 
            
            _level.Player.OnWin += OnWin;
            _level.Player.OnDead += OnDead;
        }

        private void OnDead()
        {
            StartCoroutine(FailWithDelay());  // запуск карутины (карутины привязаны к компаненту, если я выключу GameManager, то карутира также откл)
            //StopCoroutine(); - останавливает карутину
            //StopAllCoroutines(); - остановить все карутины (данный случай в компаненте GameManager)
        }

        private void OnWin()
        {
            /*_coroutine = */StartCoroutine(WinWithDelay());
            /*StartCoroutine(WinWithDelay());
            StartCoroutine(WinWithDelay());
            StartCoroutine(WinWithDelay());
            StartCoroutine(WinWithDelay());
            StopCoroutine(_coroutine);*/ // остановить карутину можно только по ссылке ... но это когда их ДОХРЕНА.. и я хочу остановить какуе-то из них.. вот и все(а ну и можно в массив их засунуть)
        }

        private IEnumerator WinWithDelay() // карутина это соПрограмма которая запустилась ПАРАЛЛЕЛЬНО нам!!!!!!!!!!!!!!
        {
            yield return ClearDelay();
            StartLevel();
            // yield return null - дождаться окончание (Update)! 
            // yield return  new WaitForEndOfFrame() - дождаться окончание кадра
            // yield return new WaitForFixedUpdate() - дождаться окончание когда отработет физический движок
            // yield return new WaitForSecondsRealtime(); - ждать секунды телефона(дивайса)
            // yield return new WaitWhile(() => _delay == 1); - пока условие(истина) _delay == 1, то код остановиться (или любое другое условие) т.е если условие будет (false) то мы выйдем из его.
            // yield return new WaitUntil(() => _delay != 1)); - (на оборот WaitWhile) т.е пока тут false, то код будет работать...
            // yield return StartCoroutine(FailWithDelay()); - ждемс... когда отработает след. карутина и дальше будет работать код что будет написан ниже (в IEnumerator WinWithDelay())
            
            /*while (expression < 1)
            {
                expression += Time.deltaTime; - эта запись значит, что один "ТИК"(yield return null) Update будет происходить expression += Time.deltaTime, пока expression < 1
                yield return null;
            }*/
        }

        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay(); // запуск карутины тип как StartCoroutine(ClearDelay());
            StartLevel();
        }

        private IEnumerator ClearDelay()
        {
            yield return  new WaitForSeconds(_delay);
            _level.Player.OnWin -= OnWin;
            _level.Player.OnDead -= OnDead;
        }
        
        private void SaveData()  // система сохранения (установить)
        {
           // PlayerPrefs.SetInt(SAVE_LEVEL, _currentLevelIndex); 
        }

        private void LoadData() // система сохранения (записать)
        {
           // _currentLevelIndex = PlayerPrefs.GetInt(SAVE_LEVEL, 0);
        }
    }
}