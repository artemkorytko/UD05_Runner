using System;
using System.Collections;
using UnityEngine;

namespace Runner_
{
    public class GameManager : MonoBehaviour
    {
        
        [SerializeField] private Level levelPrefab;
        [SerializeField] private float delay = 1f;

        private Level _level;
        private UiController _uiController; // создание ссылки на юайконтроллер (но он пока что пустой)


        public event Action ImWin;
        public event Action ImDead;
        private void Awake()
        {
            _uiController = FindObjectOfType<UiController>(); //присвоение значения в ссылку юай контроллера
           // _level = Instantiate(levelPrefab, transform);
          
            
        }

        private void Start()
        {
         // StartLevel();
         _uiController.StartGameFromScratch += GameStarting;//подписка события StartGameFromScratch из юайконтроллера на метод GameStarting
         _uiController.StartNextLevelAfterWining += GameStarting;
         _uiController.ClickOnButtonOnFailPanelToMoveToGamePanelForRestart += RestartLevelAfterDeath;
         // _uiController.

        }

        private void OnDestroy()
        {
           _uiController.StartGameFromScratch -= GameStarting;
           _uiController.StartNextLevelAfterWining -= GameStarting;
           
           _uiController.ClickOnButtonOnFailPanelToMoveToGamePanelForRestart -= RestartLevelAfterDeath;
        }

        private void GameStarting()
        {
            if (_level != null) //если уровень уже существует то зайдет в иф
            {
                Destroy(_level.gameObject); // и удалит объект уровень
            }
            _level = Instantiate(levelPrefab, transform); // и когда вышел из ифа/ или вообще в него не заходил =>создает уровень Instantiate
            StartLevel();

        }

        private void StartLevel() //старт уровня 
        {
            _level.GenerateLevel();
            _level.Player.IsActive = true;
            _level.Player.OnWin += OnWin; // подписка на метод OnWin
            _level.Player.OnDead += OnDead;
        }

        private void RestartLevelAfterDeath() //рестарт уровня
        {
            _level.ContinueLevelAfterFail();
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

        private IEnumerator WinWithDelay() //корутина, для задержки дейтсвия 
        {
            yield return ClearDelay();
           // StartLevel();
            // yield return null; //дождаться окончание апдейта
            // yield return new WaitForFixedUpdate(); //дождаться пока отработает физический движок (ждем момента пока обработается вся физика(коллизии напрмиер) и потом отрботает корутина)
            ImWin?.Invoke();
        }
        
        
        
        private IEnumerator FailWithDelay()
        {
            yield return ClearDelay();
            //StartLevel();
            ImDead?.Invoke();
        }
        
        private IEnumerator ClearDelay()
        {
            yield return new WaitForSeconds(delay);
            
            _level.Player.OnWin -= OnWin; // подписка на метод OnWin
            _level.Player.OnDead -= OnDead;
            
            
        }
    }
    
}