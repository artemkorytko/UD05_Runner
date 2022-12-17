using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner_
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private GamePanel _gamePanel;
        [SerializeField] private WinPanel _winPanel;
        [SerializeField] private FailPanel _failPanel;
        [SerializeField] private GameManager _gameManager;

        public event Action StartGameFromScratch;
        public event Action StartNextLevelAfterWining;
        public event Action ClickOnButtonOnFailPanelToMoveToGamePanelForRestart;
        
       private enum Panels
        {
            Menu,
            Win,
            Game,
            Fail

        }

       public void Start()
       {
           SwitchPanel(Panels.Menu);
           
           //_failPanel.Restart += RestartAfterClickOnFailPanel;
           //_gamePanel.
           _menuPanel.Start += StartGameFromScratchAndFromEachWinPanel; /*подписка на метод StartGameFromScratchFromEntryPanel 
          в котором будет описано что произойдет после клика на кнопку Старт (событием Start (кнопкой начинания игры из входной панели Меню)), событие Start сработает при клике
          на кнопку Старт на панели Меню тк будет висеть на кнопке
           */
           //_winPanel.NextLevel += StartNextLevelAfterWin;
           _gameManager.ImWin += ShowingOfWinPanel;
           _gameManager.ImDead += ShowingOfFailPanel;
           _winPanel.NextLevel += StartGameFromScratchAndFromEachWinPanel;
           _failPanel.Restart += RestartOfLevelAfterDeath;
           
       }


       private void OnDestroy()
       {
           _menuPanel.Start -= StartGameFromScratchAndFromEachWinPanel;
           _gameManager.ImWin -= ShowingOfWinPanel;
           _gameManager.ImDead -= ShowingOfWinPanel;
           _winPanel.NextLevel -= StartGameFromScratchAndFromEachWinPanel;
       }

       private void StartGameFromScratchAndFromEachWinPanel() //метод который будет висеть на кнпоке Start для начала игры из МенюПанели
       {
           SwitchPanel(Panels.Game); /*передаем сюда включение панели Гейм (в методе StartGameFromScratchFromEntryPanel который 
           сработает после клика на кнопку Старт на панели Меню, т е в методе StartGameFromScratchFromEntryPanel происходит
           переход на панель Game, а сам метод срабатывает при клике на кнопку старт(см метод void Start). а метод StartGameFromScratchFromEntryPanel 
           висит на кнопке
       */
           StartGameFromScratch?.Invoke();
       }

       private void ShowingOfWinPanel()
       {
           SwitchPanel(Panels.Win);
           //ImOnWinPanel?.Invoke();

       }
       
       private void ShowingOfFailPanel()
       {
           SwitchPanel(Panels.Fail);
           
       }

       // private void StartNextLevelAfterWin()
       // {
       //     SwitchPanel(Panels.Game);
       //     StartNextLevelAfterWining?.Invoke();
       // }

       private void SwitchPanel(Panels panel)
       {
           _menuPanel.gameObject.SetActive(panel == Panels.Menu); //.gameObject - обращаюсь ко всему объекту MenuPanel на сцене (через ссылку на него _menuPanel)
           _gamePanel.gameObject.SetActive(panel == Panels.Game);
           _winPanel.gameObject.SetActive(panel == Panels.Win);
           _failPanel.gameObject.SetActive(panel == Panels.Fail);
       }

       private void RestartOfLevelAfterDeath()
       {
           ClickOnButtonOnFailPanelToMoveToGamePanelForRestart?.Invoke(); // инвоук срабатывающий при нажатии на кнопку для перехода для Гейм ПАнел
           SwitchPanel(Panels.Game);
       }
       
    }

}