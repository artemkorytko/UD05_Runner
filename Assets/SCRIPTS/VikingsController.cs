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

//================ КОНТРОЛЛЕР ВСЕЙ СЦЕНЫ ТУТ ============================================
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

    
    public class Gold
    {
        public Vector3 G_position;

        public Gold(Vector3 gPosition) // че пишууууу?
        {
            G_position = gPosition;
        }
    }
    

    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _goldprefab;
    [SerializeField] private GameObject _vikingprefab;
    [SerializeField] private GameObject _door;
    
    [SerializeField] private float _padding = 0.5f; // расстановка по сетке
    
    public List<Viking> _vikingsArray; // инит массива для викингов
    public List<Gold> goldarray; //или Array???
    public List <float> V_Door_dist ;

    private VikingKorabl _vikingKorablfile; // подключение файла с кораблем
    int howmanyvikings = 13;

    private VikingsController _thisfile;

    private int ord_no;

    private event Action VikingiVyshli;

    //---------------- массив цветов для покраски викингов (шоб разные были) ----------------------- 
    // пока не работет
    private static List<string> colorlist = new List<string>()
        { "green", "white", "blue", "pink", "yelow", "cyan", "magenta", "black" };
    public int howmanycolors = colorlist.Count;

    
    //-----------------------------------------------------------------------------------------------
    private void Awake()
    {
        _vikingKorablfile = FindObjectOfType<VikingKorabl>();
        _vikingKorablfile.Priplyl += VikingiIzKorablya; //подписка на приплытие изнутри корабля
        
        _thisfile = FindObjectOfType<VikingsController>(); 
    }


    //-----------------------------------------------------------------------------------------------
    void Start()
    {
        LayGold();
        
        // по концу цикла расстановки викингов долго сработать событие ВикингиВышли
        _thisfile.VikingiVyshli += VikingiToCHurch;
        
        
        
    }
    
    //-----------------------------------------------------------------------------------------------
    private void LayGold()
    {
        goldarray = new List<Gold>(howmanyvikings);
        
        int ord_no_gold = 0; // для слоев 
        var _gstartX = -1.2f;
        var _gstartY = 1f;
        int RowLength = 7;

        //--------- разложить золото ------------------------------------------------------------------------------
        for (int i = 0; i < howmanyvikings; i++)
        {
            _gstartX  = _gstartX + _padding; //типо сдвинет вправо
            
            if (i == RowLength) // переход на вторую строчку
            {
                _gstartY = _gstartY - _padding * 0.8f; //как есть
                _gstartX = -1.2f + _padding;
            }
            
            Vector3 g1Pos = new Vector3(_gstartX, _gstartY, 0);
            
            // ---- в массив идет только свежерассчитанная позиция 
            goldarray.Add( new Gold(g1Pos));
            
            // !!----- внимание, использует координаты из массива !!! ---------
            var thisGold = Instantiate(_goldprefab, goldarray[i].G_position, Quaternion.identity, transform);
            ord_no_gold++;
            thisGold.GetComponent<SpriteRenderer>().sortingOrder = 150 + ord_no;
        }
    }

    
    
    

    //---------------------- викинги выходят ------------------------------------------------
    private void VikingiIzKorablya()
    {
        _vikingsArray = new List<Viking>(howmanyvikings);

        //------- стартовая позиция первого викинга
        var _vstartX = -4.5f;
        var _vstartY = -1.5f;

        int RowLength = 4;
        //int HowmanyColumns = 3;
         // <----сколько викингов !!!!
        string thisvikingcolorname = null; // для рандомного цвета 
        
        //=========== цикл делает викингов, сует в массив и расставляет в стартовую позицию ==========================
        for (int i = 0; i < howmanyvikings; i++)
        {
            //------------ выбирает рандомный цвет из массива (не могу всунуть в покраску) --------------
            int thisvikingcolor = Random.Range(0, howmanycolors);
             thisvikingcolorname = colorlist[thisvikingcolor].ToString();
            
            //------------ расстановка рядочками (можно наверное изящней, но как есть)-------------------
            _vstartX  = _vstartX + _padding; //типо сдвинет вправо
            
            if (i == RowLength) // переход на вторую строчку
            {
                _vstartY = _vstartY - _padding * 2; //как есть
                _vstartX = -4f;
            }

            if (i == RowLength * 2)
            {
                _vstartY = _vstartY - _padding * 2.5f; //тут пока херь
                _vstartX = -4f;
            }

            //------------- заполняет массив -------------------------------------
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
                
                // Order! ------!!! номер в слое !!!--------------------------
                ord_no++;
                thisViking.GetComponent<SpriteRenderer>().sortingOrder = 100 + ord_no;
                
                //--------- красим
                var paintornottopaint = Random.Range(0, 3);
                if (paintornottopaint == 0)
                {
                    thisViking.GetComponent<Renderer>().material.color = UnityEngine.Color.cyan;
                }
                if (paintornottopaint == 1)
                {
                    thisViking.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
                }
                // Color.FromName(thisvikingcolor); - тоже не работает
                // ????????????????????????????????????????????????????????
                // ????????? КАК всунуть стринговую переменную в команду Color??? ??????????????????????
                // ???????????????????????????????????????????????????????
               
                
                // Debug.Log(" тадам "); // ==> ОНО ОТКЛАДЫВАЕТ ЭТО ДЕЙСТВИЕ НА 3с !!!!!!!!!!!!
           }
           
           
        }// конец цикла for
        VikingiVyshli?.Invoke();
    }
    //=================================================================================================================

    void VikingiToCHurch()
    {
        
        float vdoordist = 0; // переменная для высчитывания
        
        //get position of the door
       Vector3 doorpos = _door.transform.position;
       
       // небольшой массив для дистанций от викингов к двери
       V_Door_dist = new List<float>();


       // item ибо у нас класс
       foreach (Viking item in _vikingsArray)
       {
           
           // !!!!
           Vector3 xx = item.V_position; 
           
           //каждому викингу высчитываем дистанцию
           vdoordist = Vector3.Distance(xx, doorpos);
           
           //суём дистанцию в массив
           V_Door_dist.Add(vdoordist);
           Debug.Log($"Dist for Viking {_vikingsArray.IndexOf(item)} -- {vdoordist}");
       }
       
       // перебрть новый массив и найти меньшее?
       
       
       
       //List.Sort(V_Door_dist);
       
       // foreach (float distinarr in V_Door_dist)
       //         {
       //             
       //         }
       
       
    }

    
        
    

    void Update()
    {
        
    }//update end


    private void OnDestroy()
    {
      //  _vikingKorablfile.Priplyl -= VikingiIzKorablya;
    }
}

