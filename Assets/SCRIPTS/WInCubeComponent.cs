using UnityEngine;

namespace Runner 
{
    // ПОКА КУБ НЕ БЫЛ В ПРЕФАБЕ - РАБОТАЛО, НИД ХЕЛП!!!!
    
    public class WInCubeComponent : MonoBehaviour
    {
        //[SerializeField] private GameObject _finishpart;
        [SerializeField] private ParticleSystem _winparticles;

        private PlayerController _playerControllerFile;
        
        void Awake()
        {
            
            _winparticles = GetComponentInChildren<ParticleSystem>();
            
            _playerControllerFile = FindObjectOfType<PlayerController>(); // вечно это забываю ((((
            _playerControllerFile.Dobezal += StartWinParticlesBurst;
        }

        void StartWinParticlesBurst()
        {
            Debug.Log("событие Dobezal передано в куб");
            _winparticles.Play(); //------ работало без префаба или если управлять из Level
        }
    }
}