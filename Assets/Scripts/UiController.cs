using System;
using UnityEngine;

namespace Runner
{
    public class UiController : MonoBehaviour
    {
        private enum PanelType
        {
            None, Menu, Game, Win, Fail 
        }

        private GameManager _gameManager;
        
        private MenuPanel _menuPanel;
        private GamePanel _gamePanel;
        private WinPanel _winPanel;
        private FailPanel _failPanel;
        
        public event Action OnStartButtonEvent;
        public event Action OnNextButtonEvent;

        private void Awake()
        {
            _menuPanel = GetComponentInChildren<MenuPanel>();
            _gamePanel = GetComponentInChildren<GamePanel>();
            _winPanel = GetComponentInChildren<WinPanel>();
            _failPanel = GetComponentInChildren<FailPanel>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            _gameManager.OnWinn += OnWin;
            _gameManager.OnFAill += OnFail;
            _menuPanel.OnStartButtonClick += OnStartButtonClick;
            _winPanel.OnNextButtonClick += OnNextButtonClick;
            _failPanel.OnNextButtonClick += OnNextButtonClick;
            SwitchPanel((PanelType.Menu));
        }

        private void OnFail()
        {
            SwitchPanel(PanelType.Fail);
        }

        private void OnWin()
        {
            SwitchPanel(PanelType.Win);
        }

        private void OnDestroy()
        {
            _gameManager.OnWinn -= OnWin;
            _gameManager.OnFAill -= OnFail;
            _menuPanel.OnStartButtonClick -= OnStartButtonClick;
            _winPanel.OnNextButtonClick -= OnNextButtonClick;
            _failPanel.OnNextButtonClick -= OnNextButtonClick;
        }
        
        private void OnNextButtonClick()
        {
            OnNextButtonEvent?.Invoke();
            SwitchPanel(PanelType.Game);
        }

        private void OnStartButtonClick()
        {
            OnStartButtonEvent?.Invoke();
            SwitchPanel(PanelType.Game);

        }

        private void SwitchPanel(PanelType panelType)
        {
            _gamePanel.gameObject.SetActive(panelType == PanelType.Game);
            _menuPanel.gameObject.SetActive(panelType == PanelType.Menu);
            _winPanel.gameObject.SetActive(panelType == PanelType.Win);
            _failPanel.gameObject.SetActive(panelType == PanelType.Fail);
        }
    }
}