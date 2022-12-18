using System;
using System.Collections;
using Unity.VisualScripting;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Runner
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Level levelPrefab;

        //поле для задержек в корутинах:
        [SerializeField] private float delay = 1f;

        private Level _level;

        private void Awake()
        {
            _level = Instantiate(levelPrefab, transform); // внутрь текущего трансформа ???
            
        }
        
        // пока на старте потом рпо кнопке
        private void Start()
        { 
            StartLevel(); //см. ниже, вынесли их чтобы не повторять. ПОТОМ ИЗ КНОПКИ
        }
        
        // выносим это в отдельный метод ибо левел генерится много раз

        private void StartLevel()
        {
            _level.GenerateLevel(); // обратились к функции внутри Level или к компонету на созданном левеле!!!????
            
            // надо запустить игрока 
            _level.Player.IsActive = true;
                     
            // подписки на события из плеера в PlayerController, ибо он там пересекает и стукается
            _level.Player.Dobezal += OnWin; // у АК тут опять одинаковые слова
            _level.Player.OnDie += OnDead;

            // Начать звук бега: прям сразу ищет компонент и в нем запустили функцию О_о
            FindObjectOfType<AudioManager>().InitSound();
            
            
        }

        //------------ методы по событиям --------------------------------------------------------------------------
        private void OnDead()
        {   // надо сделать задержку для того чтобы посмотрели анимацию -> корутины
            StartCoroutine(FailWithdelay());
        }

        
        private void OnWin()
        {
            // надо сделать задержку для того чтобы посмотрели анимацию > корутины
            StartCoroutine(WinWithdelay());
        }

        
        //------- КОРУТИНЫ для задержек - она остановит программу и будет ждать пока отработиает Иелдоператор ------- 
        private IEnumerator WinWithdelay()
        {
            yield return ClearDelay(); 
            
            // StartLevel(); <---- убрали и стало отлично работать
           
            // без UI мы хотим запустить новый уровень
            // надо: отписаться от старых событий

        }
        
        private IEnumerator FailWithdelay()
        {
            yield return ClearDelay(); 
            
          //  StartLevel(); // StartLevel(); <---- убрали и стало отлично работать
        }
        //-----------------------------------------------------------------------------------------------------------
        
        
        //--------------вынесли отписки и паузу-----------------------------
        // из-за того, что и на победу и на стук об стену - вызываем одинаковые
        // отписки от уровня и генерацию нового - выносим это в отдельную корутинку
        private IEnumerator ClearDelay()
        {
            // отписаться
            _level.Player.Dobezal -= OnWin; 
            _level.Player.OnDie -= OnDead;
            
            // код отсновит выполнение в этом методе, пока не выполнится это условие
            yield return new WaitForSeconds(delay);
            
            // и запустить новый уровень
            StartLevel(); 
        }
    }
}