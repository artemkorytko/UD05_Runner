using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Runner;
using TMPro;
using UnityEngine;

namespace Runner
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text leveltext;
        [SerializeField] private TMP_Text coinstext;

        private Level _levelfile;
        private GameManager _gamemanagerfile;

        // ----------------------------------

        private void Awake()
        {
            _gamemanagerfile = FindObjectOfType<GameManager>();
            _gamemanagerfile.LevelChanged += PrintLevelText;
        }

        public void PrintLevelText()
        {
            _levelfile = FindObjectOfType<Level>();
            string whattoprintIntoLevel = _levelfile.currentlevel.ToString();

            leveltext.text = $"Level {whattoprintIntoLevel} ";
        }

        private void OnDestroy()
        {
            _gamemanagerfile.LevelChanged -= PrintLevelText;
        }
    }
}