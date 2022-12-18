using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class AudioManager : MonoBehaviour
    {
        // поля куда потянем звуки руками
        [SerializeField] private AudioClip winsound;
        [SerializeField] private AudioClip fallsound;

        //[SerializeField] private AudioClip stepsound; - вынесла на плеера новый аудиосурс
        [SerializeField] private AudioClip coinsound;

        private bool Topat = true;
        private float stepSpeed = 0.4f;
        private AudioSource _audioSource;
        private AudioSource _playerstepsound;

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
            _playercontrollerfile.GetCoin += CoinSoundFunction;
            Topat = true;
            
            _playerstepsound = _playercontrollerfile.GetComponent<AudioSource>();
            StepSoundFunction();
        }

       


        //------------ отдельные функции со звуками --------------------
        private void WinSoundFunction()
        {
            Topat = false;
            
            _audioSource.PlayOneShot(winsound);
            //Debug.Log("Звук победы");
        }


        private void FallSoundFunction()
        {
            Topat = false;
            _audioSource.PlayOneShot(fallsound);
            //Debug.Log("Звук упал");
        }

        
         private void CoinSoundFunction()
         {
            _audioSource.PlayOneShot(coinsound);
            Debug.Log("Монетка бздынь");
         }
         
         
         //-------------------------- шаги ----------------------------------------
        private void StepSoundFunction()
        {
            StartCoroutine(StepStep());

            IEnumerator StepStep()
            {
                yield return new WaitForSeconds(0.3f); // пока разгоняется
                
                while (Topat) // бесконечный цикл топания
                {
                    _playerstepsound.pitch = Random.Range(0.8f, 1.2f); // не унылая тональность топания
                    _playerstepsound.Play();
                    yield return new WaitForSeconds(stepSpeed);
                }
            }
        }
    } // close class
}