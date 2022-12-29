using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_PriestController : MonoBehaviour
{
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _kandelabr;
    

    private float moveSpeed = 10f;
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
            // машет канделябром
            Debug.Log("вжух");
            // добавить коллайдер на канделябр, rigibody на викинга
        }
    }
}
