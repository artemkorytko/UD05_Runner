using System;
using System.Collections;
using System.Collections.Generic;
using Runner_;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f; // то с какой скоростью мы двигаемся
    [SerializeField] private float sideSpeed = 5f;
    [SerializeField] private float roadWidth = 5f; //ширина нашей дороги
    [SerializeField] private float turnRotationAngle = 30f;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private Transform model; // будем поворачивать нашу модель(всего логического Player'a(не перса отдельно))
    
    private Rigidbody _rigidbody;
    private Animator _animator; //получаем ссылку на аниматор чтобы иметь возможность завести нашу анимацию
    private InputHandler _inputHandler; //получаем ссылку на инпутхэндлер чтобы считать какие у него есть данные 

    private bool _isActive; //флаг //_isActive по умолчанию false)
    private static readonly int Run = Animator.StringToHash("Run"); // берет строку(string) "Run" переводит ее в хэш и помещает этот хэш внутрь переменной Run//хэш(конверсия) строчки _animator.SetTrigger(Run);
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Dance = Animator.StringToHash("Dance");

    public event Action OnWin;  //создали ивент чтобы сделать инвоук
    public event Action OnDead;
   
    //суть геттера и сеттера в том что считать значение IsActive можем из любого класса, а устанавливаем значение именно тут
    public bool IsActive //флаг который будет говорить что изменилось состояние нашего перса(напр умер, финиш игры),
                         //этот флаг засунем в методы где флаг и будет менять свое значение
    {
        //IsActive публичный то есть значение считыватся из любого класса
        get => _isActive; //getter вернет _isActive  // считывает значение _isActive'a из любых классов и методов тк флаг bool IsActive публичный

        set //set устанавливает логику (которая связана с тем что флаг bool поменял свое значение с исходного false на какое-то другое_
        {
            _isActive = value; //=>если флаг не false  _isActive принимает значение
            if (_isActive) //если флаг тру то отработает сет триггер ран
            {
                _animator.SetTrigger(Run); //аниматор устанавливает значение Run //делали выше конверсию стринга в хэш чтобы получился Run  не стрингом
            }
        }
    }
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>(); // аниматор - дочерний объект player'a (который перс, а не логический player отвечающий за логику)во вьюшке => находим на эвэйке и вызыываем его как чайлда
        _inputHandler = GetComponent<InputHandler>(); //инпутхэндлер висит на нашем объекте => получаем ссылку как геткомпонент
        _rigidbody = GetComponent<Rigidbody>(); // вешаем в инспекторе

    }

    // private void Start()
    // {
    //     IsActive = true;
    // }
    //

    private void FixedUpdate() //метод для бега т к в нем ссылка на Move
    {
        if (!IsActive) //зайдет в иф если выполняется условие !_isActive //т е зайдет в иф если мы не активны и как следствие не дойдет до move
            return; // return елси зашел в иф и как следствие не дойдет до мув
        Move(); // сюда зайдет если не зашел в иф //то есть зайдет в мув если мы активны
    }

    private void Move()
    {
        //Debug.Log("Here");
        var xOffset = - _inputHandler.HorizontalAxis * sideSpeed; //xOffset - на сколько нам надо сместиться(на сколько нащ палец смещается относительно ширины дороги)
        var position = _rigidbody.position;
        position.x += xOffset; // к позиции по х прибваляем + Offset (т е на сколько нам надо сместиться на экране относ.....
        position.x = Mathf.Clamp(position.x, -roadWidth * 0.5f, roadWidth * 0.5f);

        var rotation = model.localRotation.eulerAngles; //поворачиваем нашу модель(объект) относителдьно системы координат пэрэнта на угол  
      /* if (_inputHandler.IsHold && Mathf.Sign(xOffset) > 0)
        {
          //  Quaternion q = Quaternion.AngleAxis(30, Vector3.left);
            //model.rotation = Quaternion.Slerp(model.rotation, q, lerpSpeed * Time.deltaTime);
          //  _rigidbody.AddTorque(transform.localEulerAngles*30f);
          Vector3 mov = new Vector3(position.x, position.y*(0.5f), position.z);
        }
        if (_inputHandler.IsHold && Mathf.Sign(xOffset) < 0)
        {
            // Quaternion q = Quaternion.AngleAxis(30, Vector3.right);
          //  model.rotation = Quaternion.Slerp(model.rotation, q, lerpSpeed * Time.deltaTime);
           // _rigidbody.AddTorque(transform.right*30f);
           Vector3 mov = new Vector3(position.x, position.y* 0.5f, position.z);
        } */
       
      //  rotation.y = 
         rotation.y = Mathf.LerpAngle(rotation.y, _inputHandler.IsHold  ? Mathf.Sign(xOffset) * turnRotationAngle : 0, lerpSpeed * Time.deltaTime);
        /* поворачиваем относительно оси у. LerpAngle(интерполяция, то есть двигаемся от начального до конечного угла с заданным шагом ( от rotation.y до _inputHandler.IsHold)
         Mathf.Sign(xOffset) - берем знак нашей переменной xOffset (если xOffset = -2525252 => вернет (-1); если xOffset = +0,00004 => вернет +1) */
        model.localRotation = Quaternion.Euler(rotation); //новый угол присваиваем нашей модели
        
        _rigidbody.MovePosition( position  + transform.forward * (forwardSpeed * Time.deltaTime));/*
       // к текущей позиции position нашего твердого тела rigidbody прибавляем вектор направления(transform.forward - вектор кот указывает направление)который умножен * на Time.deltaTime
       //  */
    }

    private void OnCollisionEnter(Collision collision) /*OnCollisionEnter встроенный юнити метод- вызывается когда 
    случилось соударение //для коллизии нужны коллайдеры //collision - сюда вернется тот объект с которым мы сколлизились */
    {
        if (collision.gameObject.GetComponent<WallComponent>()) /*проверка на соударение(коллизию) 
        // вернется тру или фолс: тру когда получилось получить компонент, фолс - когда не получилось получить 
        компонент WallComponent//отправляем Wallcomponent - компонент который висит на стене => */
        {
            Died(); // если ударились об стену т е получили компонентт воллкомпонент => условие if'a выполнилось => следовательно попали в метод и заактивился метод Died()
        }
    }
    
    private void OnTriggerEnter(Collider other) //OnTRiggerEnter - т к финиш является триггером(триггер потому что не коллижн, коллижн когда ударились об него и оттолкнулись, иначе - триггер) (т е мы коснулись нашего триггера - финиша)
    {
        if (other.gameObject.GetComponent<FinishComponent>()) /*FinishComponent - jтдельный класс который повесим на
            объект чтобы он мог существовать и этот параметр можно было применять в логине//проверка на соударение(коллизию) /
             / вернется тру или фолс: тру когда получилось получить компонент, фолс - когда не получилось получить компонент 
             WallComponent//отправляем Wallcomponent - компонент который висит на стене => 
            */
        {
            Finish(); // если коснулись триггера (то на что повесим FinishComponent) т е получили компонент воллкомпонент след-но
        }
    }

    [ContextMenu("Died")] /*добавляю атрибут ContextMenu может создавать дебажное обращение к методу из
     редактора (т е в Юнити можно будет обратиться через инспектор и выбрать этот метод напр с целью проверить 
     нормально ли отрабатывает метод(в этом случае нормально ли отрабатывает метод Died
     */
    
    private void Died()
    {
        IsActive = false; //на смерти перестали быть активными
        _animator.SetTrigger(Fall); // в методе Died устанвлииваем триггер соответствующий смерти т е Fall//а аниматору присваиваем ( вызываем) анимацию fall
        OnDead?.Invoke();
    }
    
    [ContextMenu("Finish")]
    private void Finish()
    { 
        IsActive = false; //на финише перестали быть активными
        _animator.SetTrigger(Dance); // в методе finish устанвлииваем триггер соответствующий окончанию игры т е Dance//  !!! Dance - это название точно должно совпадать с названием анимации в аниматоре в Юнити

        OnWin?.Invoke();
    }
}

//IsTrigger - объект не имеет свойства отталкивания при соударении

