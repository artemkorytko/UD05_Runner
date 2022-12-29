using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = System.Drawing.Color;
using Random = UnityEngine.Random;

public class VikingsController : MonoBehaviour
{
    [Serializable]
    public class Viking
    {
        public bool GoldShown;
        public int Color;
        public int Health;
        public Vector3 V_position;

        public Viking(bool goldShown, int color, int health, Vector3 vPosition)
        {
            GoldShown = goldShown;
            Color = color;
            Health = health;
            V_position = vPosition;
        }
    }

    [SerializeField] private GameObject _korabl;
    [SerializeField] private GameObject _vikingprefab;
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _goldprefab;

    [SerializeField] private float _padding = 0.5f; // расстановка по сетке

    float _korablstartX;

    public List<Viking> _vikingsArray;


    //---------------- массив цветов для покраски викингов (шоб разные были) ----------------------- 
    private static List<string> colorlist = new List<string>()
        { "green", "white", "blue", "pink", "yelow", "cyan", "magenta", "black" };
    public int howmanycolors = colorlist.Count;

    
    
    //-----------------------------------------------------------------------------------------------
    private void Awake()
    {
       // _korabl

    }




    void Start()
    {
        

        var korablstartX = _korabl.transform.localPosition.x;
        //var korablstartY = _korabl.transform.localPosition.y;

        StartCoroutine(Priplyzd());


        VikingiIzKorablya();
    }



    //-------------------------------------------------------------------------------------------------

    private void VikingiIzKorablya()
    {
        _vikingsArray = new List<Viking>(12);

        //------- стартовая позиция первого викинга
        var _vstartX = -1f;
        var _vstartY = 2f;
        int howmanyvikings = 12; // <----сколько викингов !!!!

        //------- цикл делает викингов, сует в массив и расставляет в стартовую позицию ------------------
        for (int i = 0; i < howmanyvikings; i++)
        {
            float xxx = _vstartX + i * _padding; //типо сдвинет вправо
            if (i == 3) // переход на вторую строчку
            {
                float yyy = _vstartY + _padding; //тут пока херь
            }

            else if (i == 6)
            {
                float yyy = _vstartY + _padding * 2; //тут пока херь
            }

            Vector3 v1Pos = new Vector3(xxx, _vstartY, 0);
            _vikingsArray.Add(new Viking(false, // пока без золота
                Random.Range(0, howmanycolors),
                100, // пока все здоровенькие
                v1Pos //
            ));
            Instantiate(_vikingprefab, _vikingsArray[i].V_position, Quaternion.identity, transform);
            //_vikingsArray[i].Color.
            
          
        }

    }
    //-------------------------------------------------------------------------------------------------

    





    //-----------------------------------------------------------------------------------------------
    private IEnumerator Priplyzd()
    {
        Debug.Log("Корутина начата");
        
        for (int i = 0; i < 40; i++)
        {
            _korabl.transform.Translate(new Vector3(_korablstartX + 0.1f, 0,0) * Time.deltaTime);
            
        }
        yield return new WaitForSeconds(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}