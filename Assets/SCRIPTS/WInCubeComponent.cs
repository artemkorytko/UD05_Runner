using UnityEngine;

namespace Runner 
{
    // ПОКА КУБ НЕ БЫЛ В ПРЕФАБЕ - РАБОТАЛО, НИД ХЕЛП!!!!
    
    public class WInCubeComponent : MonoBehaviour
    {
       [SerializeField] private ParticleSystem _winparticles;
        // private ParticleSystem _winparticles;
        private PlayerController _playerControllerFile;
        
        void Awake()
        {
            _winparticles = GetComponentInChildren<ParticleSystem>();
            
            _playerControllerFile = FindObjectOfType<PlayerController>(); // вечно это забываю ((((
            _playerControllerFile.Dobezal += StartWinParticlesBurst;
        }

        void StartWinParticlesBurst()
        {
            Debug.Log("добежал то");
            _winparticles.Play(); // 
        }
    }
}