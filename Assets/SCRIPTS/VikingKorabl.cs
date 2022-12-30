using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VikingKorabl : MonoBehaviour
{
    [SerializeField] private GameObject _korabl;
    
    public event Action Priplyl; // передается в VikingController для высадки викингов
    
    
    void Start()
    {
        Priplyzd();
        
    }

    //---------------------------- корабль пытается приплыть -------------------------------------------------------------------
    void Priplyzd()
    {
        float korabldistance = 4.2f; // неочевидный ебанутый регулятор расстояния который кажется засист
        // от нагрузки када на кадр

        StartCoroutine(WaitTillKorablComes());

        IEnumerator WaitTillKorablComes() 
        {
            // ждет конца второй корутины и запускает событие ПОСЛЕ (?)
            yield return StartCoroutine(MovekorablCorout());
            Priplyl?.Invoke(); 
        }

        IEnumerator MovekorablCorout() // корина двигает корабль
        {
            
            //????????????????????????????????????????
            // я задолбалась как сделать чтобы двигалось до нужной координаты а не куда придется?????
            // 60 - это кадры?????
            //????????????????????????????????????????
            for (int i = 0; i < 150; i++)
            {

                    //_korabl.transform.getlocalPosition.x = 3;
                    //_korabl.transform.Translate(_korabl.transform.position + new Vector3(3,3,3));
                    //_korabl.transform.Translate(Vector3.right  * korablspeed * Time.deltaTime );
                    _korabl.transform.Translate(Vector3.right * korabldistance * Time.deltaTime);
                
                    yield return null; // СЮДА НАДО БЫЛО!!!!!!!!!
            }

            yield return new WaitForSeconds(1f);
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
