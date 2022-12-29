using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Color = System.Drawing.Color;
using Random = UnityEngine.Random;

public class VikingsController : MonoBehaviour
{
    [Serializable]
    public class Viking
    {
        public bool GoldShown;
        public int VColor;
        public int Health;
        public Vector3 V_position;

        public Viking(bool goldShown, int vcolor, int health, Vector3 vPosition)
        {
            GoldShown = goldShown;
            VColor = vcolor;
            Health = health;
            V_position = vPosition;
        }
    }

    [SerializeField] private GameObject _korabl;
    
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _goldprefab;
    [SerializeField] private GameObject _vikingprefab;
    
    
    [SerializeField] private float _padding = 0.5f; // расстановка по сетке

    public float _korablstartX;

    public List<Viking> _vikingsArray;
    //private Renderer _vikingrenderer;


    //---------------- массив цветов для покраски викингов (шоб разные были) ----------------------- 
    private static List<string> colorlist = new List<string>()
        { "green", "white", "blue", "pink", "yelow", "cyan", "magenta", "black" };
    public int howmanycolors = colorlist.Count;

    
    
    //-----------------------------------------------------------------------------------------------
    private void Awake()
    {
       // _vikingrenderer = _vikingprefab.GetComponent<Renderer>();

    }




    void Start()
    {
        
       
        // корабль не работает
        // var korablstartX = _korabl.transform.localPosition.x;
        // var korablstartY = _korabl.transform.localPosition.y;
        Priplyzd();
        


        VikingiIzKorablya();
        VikingiToCHurch();
    }



    //-------------------------------------------------------------------------------------------------

    private void VikingiIzKorablya()
    {
        _vikingsArray = new List<Viking>(13);

        //------- стартовая позиция первого викинга
        var _vstartX = -4.5f;
        var _vstartY = -1.5f;

        int RowLength = 4;
        //int HowmanyColumns = 3;
        int howmanyvikings = 13; // <----сколько викингов !!!!

        //------- цикл делает викингов, сует в массив и расставляет в стартовую позицию ------------------
        for (int i = 0; i < howmanyvikings; i++)
        {
            int thisvikingcolor = Random.Range(0, howmanycolors);
            string thisvikingcolorname = colorlist[thisvikingcolor].ToString();
            
            _vstartX  = _vstartX + _padding; //типо сдвинет вправо
            
            if (i == RowLength) // переход на вторую строчку
            {
                _vstartY = _vstartY - _padding * 2; //как есть
                _vstartX = -4f;
            }

            if (i == RowLength * 2)
            {
                _vstartY = _vstartY - _padding * 3; //тут пока херь
                _vstartX = -4f;
            }

            Vector3 v1Pos = new Vector3(_vstartX, _vstartY, 0);
            _vikingsArray.Add(new Viking(false, // пока без золота
                thisvikingcolor,
                100, // пока все здоровенькие
                v1Pos //
            ));
            
           // это нормально вообще, корутина в цикле? Иначе тупо откладывает все сразу :/
           int secs = i;
           StartCoroutine(WaitaSec()); 
           
           IEnumerator WaitaSec()
           {
                float waittime = secs * 0.2f; // задержка между появлением викингов
                yield return new WaitForSeconds(waittime);
                
                //---------- штампуем викингов -------------------------------
                var thisViking = Instantiate(_vikingprefab, _vikingsArray[secs].V_position, Quaternion.identity, transform);
                // ??????????????????????????????????????????????????????
                // ????????? КАК ЗАДАТЬ НОМЕР СЛОЯ ??????????????????????
                // ???????????????????????????????????????????????????????
                
                //--------- красим
                var rndcolor = (Random.Range(0, howmanycolors));
                thisViking.GetComponent<Renderer>().material.color = UnityEngine.Color.cyan; 
                // Color.FromName(thisvikingcolorname);
                // ????????????????????????????????????????????????????????
                // ????????? КАК всунуть стринговую переменную в команду Color??? ??????????????????????
                // ???????????????????????????????????????????????????????
                
                
                // Debug.Log(" тадам "); // ==> ОНО ОТКЛАДЫВАЕТ ЭТО ДЕЙСТВИЕ НА 3с !!!!!!!!!!!!
           }
        }// конец цикла for
    }
    //-------------------------------------------------------------------------------------------------

    void VikingiToCHurch()
    {
        //_vikingsArray[1].V_position ээээ блин;
    }

    
        
    //---------------------------- корабль пытается приплыть -------------------------------------------------------------------
    void Priplyzd()
    {   
        float korablspeed = 10f;
        // Debug.Log("Корутина начата"); // это все была корутина но теперь тупо функция
        
        for (int i = 0; i < 40; i++)
        {
                
            //_korabl.transform.getlocalPosition.x = 3;
            //_korabl.transform.Translate(_korabl.transform.position + new Vector3(3,3,3));
            //_korabl.transform.Translate(Vector3.right  * korablspeed * Time.deltaTime );
            _korabl.transform.Translate(3, 0, 0);
            // ?????????????????????????????????????????????????????????????????????????????????
            // ?????????????????? не плывет. Я не понимаю, как использвать этот транслейт : ((((
            // может еще компомнет какой искать надо было ??????????????????????????????
            // ?????????????????????????????????????????????????????????????????????????????????
        }




    // Update is called once per frame
    void Update()
    {
        
        }

    }//update end

}