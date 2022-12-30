using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_PriestController : MonoBehaviour
{
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _kandelabr;

    private Rigidbody _priestrigidbody;
    private Rigidbody _kandrigidbody;

    private float moveSpeed = 10f;
    private bool zamah = true; // флаг для поднятия канделябра
    
    private bool isPressedSpace = false; // флаг нажатого пробела
    
    public event Action BumPoBashke;

    private void Awake()
    {
        // найти внутренний ригибади у канделябра
        _priestrigidbody = GetComponent<Rigidbody>();
        _kandrigidbody = _kandelabr.GetComponentInChildren<Rigidbody>();
        
        // поставить по событию "1й викинг вошел!" потом!!!!!!!!!!!!!!!!
        if (zamah)
        {
            _kandelabr.transform.Rotate(0, 0, -90);
        }        
        
         
    }

    //
    private void StayInChurch()
    {
        var position = _priest.transform.position;
        
        float priestXbounds = _priestrigidbody.position.x;
        priestXbounds = Mathf.Clamp( priestXbounds,-1.1f,3.77f);

        float priestYbounds = _priestrigidbody.position.y;
        priestYbounds =   Mathf.Clamp( priestYbounds,-1.29f,0.57f);
                    
    }
    

    private void Update()
    {
        StayInChurch();
        
        //----------------- кнопки двигания попа -----------------------------------------
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _priest.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.DownArrow))
        {
            _priest.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _priest.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _priest.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            // выходим, если пробел уже был нажат...
            if (isPressedSpace)
                return;
            
            // устанавливаем флаг нажатого пробела
            isPressedSpace = true;
            
            //?????????????????????????? тут бы ограничить одним разом но как?? 
            BumKandelabrom(); // машет канделябром
            
            // выходим из обработки пробела
            return;
        }

        // снимаем флаг нажатого пробела
        isPressedSpace = false;

    } //end update

    //-------------------------- функция удара канделябром по викингу -----------------------------------
    void BumKandelabrom()
    {
        // ????????????????? дебаг вызваается по 60 раз и более
        

        StartCoroutine( WaitTillPodnimet());

        IEnumerator WaitTillPodnimet()
        {
            yield return StartCoroutine(BUMS());
            zamah = true;
        }

        IEnumerator BUMS()
        {
            Debug.Log("вжух");
            _kandelabr.transform.Rotate(0, 0, 90);
            // ?????????? ПОКА НЕ РАБОТАЕТ ???????????????????????????????????
            // добавить коллайдер на канделябр, rigibody на викинга
            void OnTriggerEnter(Collider predmet) 
                    {
                       if (predmet.transform.parent.TryGetComponent(out VikingMarker bashka))
                       {
                         BumPoBashke?.Invoke(); // вызовет пропажу золота у викинга и выход из церкви
                       }
                    }

            yield return new WaitForSeconds(0.3f);
            _kandelabr.transform.Rotate(0, 0, -90);
        }
        
         
        
        
    }
    //-------------------------------------------------------------------------------------------------
}
