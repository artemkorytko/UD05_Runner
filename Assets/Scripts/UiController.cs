using System;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Runner
{
    public class UiController : MonoBehaviour
    {
        private enum PanelType
        {
            None,
            Menu,
            Game,
            Win,
            Fail
        }

        private MenuPanel _menuPanel;
        private GamePanel _gamePanel;
        private WinPanel _winPanel;
        private FailPanel _failPanel;

        private GameManager _gameManager;

        public event Action OnStartButtonClick;
        public event Action OnRestartButtonClick;
        public event Action OnNextButtonClick;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _menuPanel = GetComponentInChildren<MenuPanel>();
            _gamePanel = GetComponentInChildren<GamePanel>();
            _winPanel = GetComponentInChildren<WinPanel>();
            _failPanel = GetComponentInChildren<FailPanel>();
        }

        private void Start()
        {
            _gameManager.LevelWin += OnWin;
            _gameManager.LevelFail += OnFail;
            _menuPanel.OnStartGameButtonClick += StartGame;
            _winPanel.NextLevelButtonClick += StartNextLevel;
            _failPanel.RestartLevelButtonClick += RestartLevel;
            SwitchPanel(PanelType.Menu);
        }

        private void OnDestroy()
        {
            _gameManager.LevelWin -= OnWin;
            _gameManager.LevelFail -= OnFail;
            _menuPanel.OnStartGameButtonClick -= StartGame;
            _winPanel.NextLevelButtonClick -= StartNextLevel;
        }

        private void StartGame()
        {
            SwitchPanel(PanelType.Game);
            OnStartButtonClick?.Invoke();
        }

        private void StartNextLevel()
        {
            SwitchPanel(PanelType.Game);
            OnNextButtonClick?.Invoke();
        }

        private void RestartLevel()
        {
            SwitchPanel(PanelType.Game);
            OnRestartButtonClick?.Invoke();
        }
        
        private void OnWin()
        {
            SwitchPanel(PanelType.Win);
        }

        private void OnFail()
        {
            SwitchPanel(PanelType.Fail);
        }

        private void SwitchPanel(PanelType panelType)
        {
            _menuPanel.gameObject.SetActive(panelType == PanelType.Menu);
            _gamePanel.gameObject.SetActive(panelType == PanelType.Game);
            _winPanel.gameObject.SetActive(panelType == PanelType.Win);
            _failPanel.gameObject.SetActive(panelType == PanelType.Fail);
        }
    }
}