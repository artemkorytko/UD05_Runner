using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
    public class AudioManager : MonoBehaviour
    {
        // поля куда потянем звуки руками
        [SerializeField] private AudioClip winsound;
        [SerializeField] private AudioClip fallsound;
        [SerializeField] private AudioClip coinsound;


        private AudioSource _audioSource;

        // обращения к файлам
        // private Level _levelfile;
        private PlayerController _playercontrollerfile;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            //-------------- подписки 
            _playercontrollerfile = FindObjectOfType<PlayerController>(); // ыыыы
            _playercontrollerfile.Dobezal += WinSoundFunction;

            _playercontrollerfile = FindObjectOfType<PlayerController>(); // ыыыы
            _playercontrollerfile.OnDie += FallSoundFunction;
        }


        //------------ отдельные функции со звуками --------------------
        private void WinSoundFunction()
        {
            _audioSource.PlayOneShot(winsound);
            Debug.Log("Звук победы");
        }

        private void FallSoundFunction()
        {
            _audioSource.PlayOneShot(fallsound);
            Debug.Log("Звук победы");
        }
    }
}