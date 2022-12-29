using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_PriestController : MonoBehaviour
{
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _kandelabr;

    private Rigidbody _kandrigidbody;

    private float moveSpeed = 10f;
    private bool zamah = true; // флаг для поднятия канделябра
    
    private event Action BumPoBashke;

    private void Awake()
    {
        // найти внутренний ригибади у канделябра
        _kandrigidbody = _kandelabr.GetComponentInChildren<Rigidbody>();
        
        // поставить по событию "1й викинг вошел!" потом!!!!!!!!!!!!!!!!
        if (zamah)
                {
                    _kandelabr.transform.Rotate(0,0, -90);
                }
        
    }

    private void Update()
    {
        
        

        // кнопки двигания попа
        if (Input.GetKey(KeyCode.UpArrow))
        { _priest.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); }
        
        if (Input.GetKey(KeyCode.DownArrow))
        { _priest.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime); }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        { _priest.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime); }
        
        if (Input.GetKey(KeyCode.RightArrow))
        { _priest.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); }

        if (Input.GetKey(KeyCode.Space))
        {
            //?????????????????????????? тут бы ограничить одним разом но как?? 
            BumKandelabrom();
            
            // машет канделябром

        }
    }//end update

    //-------------------------- функция удара канделябром по викингу -----------------------------------
    void BumKandelabrom()
    {
        // ????????????????? дебаг вызваается по 60 раз и более
        Debug.Log("вжух");
                 
                 
                 // добавить коллайдер на канделябр, rigibody на викинга
                 // ????????????? хз как оно собирается работать  
                 void OnTriggerEnter(Collider predmet) // че за other?
                 {
                     if (predmet.transform.parent.TryGetComponent(out VikingMarker coin))
                     {
                         BumPoBashke(); // вызовет пропажу золота у викинга и выход из церкви
                     }
                 }
    }
    //-------------------------------------------------------------------------------------------------
}
