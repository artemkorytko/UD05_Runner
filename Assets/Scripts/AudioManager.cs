using System;
using UnityEngine;

namespace Runner
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioAddCoint;
        [SerializeField] private AudioClip _audioCollisionWall;
        [SerializeField] private AudioClip _audioCollisionFinish;

        private AudioSource _audioSource;
        private GameManager _gameManager;
     
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            _gameManager.OnAddCoint += OnAudioAddCoint;
            _gameManager.Win += OnAudioWin;
            _gameManager.Fail += OnAudioFail;
        }

        private void OnDestroy()
        {
            _gameManager.OnAddCoint -= OnAudioAddCoint;
            _gameManager.Win -= OnAudioWin;
            _gameManager.Fail -= OnAudioFail;
        }

        private void OnAudioFail()
        {
            _audioSource.PlayOneShot(_audioCollisionWall);
        }

        private void OnAudioWin()
        {
            _audioSource.PlayOneShot(_audioCollisionFinish);
        }

        private void OnAudioAddCoint(int obj)
        {
            _audioSource.PlayOneShot(_audioAddCoint);
        }
    }
}