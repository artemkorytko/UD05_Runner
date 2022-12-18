using System;
using UnityEngine;

namespace Runner
{
    public class GamePanel : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }
}