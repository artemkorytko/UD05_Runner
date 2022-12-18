using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Runner
{
    public class AudioManager : MonoBehaviour
    {
        // поля куда потянем звуки руками
        [SerializeField] private AudioClip winsound;
        [SerializeField] private AudioClip fallsound;

        [SerializeField] private AudioClip stepsound;
        //[SerializeField] private AudioClip coinsound;

        private bool Topat = true;
        private float stepSpeed = 0.4f;
        private AudioSource _audioSource;

        private PlayerController _playercontrollerfile;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void InitSound()
        {
            _playercontrollerfile = FindObjectOfType<PlayerController>();
            _playercontrollerfile.Dobezal += WinSoundFunction;
            _playercontrollerfile.OnDie += FallSoundFunction;
            Topat = true;
            StepSoundFunction();
        }


//------------ отдельные функции со звуками --------------------
        private void WinSoundFunction()
        {
            Topat = false;
            
            _audioSource.PlayOneShot(winsound);
            Debug.Log("Звук победы");
        }


        private void FallSoundFunction()
        {
            Topat = false;
            _audioSource.PlayOneShot(fallsound);
            Debug.Log("Звук упал");
        }


        private void StepSoundFunction()
        {
            StartCoroutine(StepStep());

            IEnumerator StepStep()
            {
                yield return new WaitForSeconds(0.3f); // пока разгоняется
                
                while (Topat) // бесконечный цикл топания
                {
                    _audioSource.PlayOneShot(stepsound);
                    yield return new WaitForSeconds(stepSpeed);
                }
            }
        }
    } // close class
}