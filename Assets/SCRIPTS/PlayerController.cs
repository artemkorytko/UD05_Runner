using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Runner;
//using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runner
{


    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float forwardspeed = 5f; // скорость для Move
        [SerializeField] private float sidespeed = 10f; 
        [SerializeField] private float roadWidth = 5f;
        [SerializeField] private float turnRotationAngle = 30f; // на сколько разрешаем повернуться
        [SerializeField] private float lerpSpeed = 5f; //для плавности поворота что ли??? 
        [SerializeField] private Transform model; //ссылка на модель, которую поворачиваем, задать руками !!!! :/

        private Rigidbody _rigidbody; // ссылка на ригибоди ДЛЯ ДВИЖЕНИЯ!
        private Animator _animator; // дочерний от вьюшки
        private InputHandler _inputHandler; // берет позицию из инпута


        // флаг про то что изменилось состояние активности
        public bool _isActive;
        
        // эти штуки райдер пишет автоматически поругавшись на "" в коде ниже
        // строковые значения надо хешировать, чтобы каждый раз оно не хешировало пока выполняет код
        private static readonly int Run = Animator.StringToHash("Run"); // на старте хешируем
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int Fall = Animator.StringToHash("Fall");

        public event Action Dobezal;
        // у АК -  public event Action OnWin;
        public event Action OnDie;
        
        
        // чтобы понимать, что этот флаг меняется - нужен геттер и сеттер :/ - это я вообще хреново понимаю
        public bool IsActive
        {
            // гугл: гет получает значение поля класса :/
            // гугл: функция без аргументов, которая сработает при чтении свойства
            // возвращает нам - куда? почему серенькое????????????
            get => _isActive; 
            
            // делает логику
            // гугл: сет - изменяет значение поля класса
            // гугл: сет - функция, принимающая один аргумент, вызываемая при присвоении свойства
            set
            {
                _isActive = value; //что за value???

                if (_isActive) // true 
                {
                    _animator.SetTrigger(Run); //alt enter - hash <----- передали название триггера (в аниматоре +)
                }
            }
        }

        // временно закоммитили дабы активировать из другого места !!!!!!!!
        // private void Start()
        // {
        //     IsActive = true; // чтобы сразу включилась анимация бега
        // }

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        
        // ----------- чтобы двигаться --------------------------------------------------------------------------
        private void FixedUpdate() // не зависит от частоты кадров, вызывается до update. Начнет бежать тут.
        {
            if (!_isActive)
                return; //дальше не выполняем, если не активны то не бегаем
            Move(); 
        }


        void Move()
        {
            // считывает HorizontalAxis в инпут в файле InputHandler!!!
            // минус перд всем - это то же, что домножить на -1, без этого двигался ЗЕРКАЛЬНО от того, куда свайпаем
            var xOffset = -_inputHandler.HorizontalAxis * sidespeed; 
            

            // указываем, куда надо сместиться
            var position = _rigidbody.position; // получаем текущую позицию у ригибоди чувачка
            position.x += xOffset; // и к ней прибовляем вышепролученный оффсет
            // теперь эту позицию нужно заклемпать, чтобы чувак не выбегал за пределы дороги:
            position.x = Mathf.Clamp(position.x, -roadWidth * 0.5f, roadWidth * 0.5f);
            // ширна дороги 6 ==>  и пусть бегаем между -3 и +3. СЕРЕДИНА ДОРОГИ ВЫХОДИТ НОЛЬ?
            
            
            // -------------------------поворот-----------------------------------------------------
            // ПРАВИЛО ------- < всегда сначала повороты потом движение > --------------------------!!!!!
            // берем углы модели
            var rotation = model.localRotation.eulerAngles;
            
            // поворот модели чела при повороте
            // поворачиваем вокруг вертикальной оси, поворот даже в 2д считается в кватерионах
            // берем текущие углы, 
            // получаем знак оффсета, положительный или отрицательный? - Matf.Sign
            // turn -30/+30, ноль - положительное число
            // если оффсет не равен нулю - стремимся к 30, 
            // Offset == 0 ? 0 : ------ если равен нулю - стремимся к нулю, если не равен то стремимся к 30 
            //дергается но работает:
            rotation.y = Mathf.LerpAngle(rotation.y, xOffset == 0 ? 0 : Mathf.Sign(xOffset) * turnRotationAngle, lerpSpeed * Time.deltaTime);
            
           // rotation.y = Mathf.LerpAngle(rotation.y, _inputHandler.IsHold ? Mathf.Abs(xOffset) > 0.01f ? Mathf.Sign(xOffset) * )
           //     turnRotationAngle : 0 : 0, lerpSpeed * Time.deltaTime);
            
           
            // присваеиваем полученный угол модели, переводя в кватерионы 
            model.localRotation = Quaternion.Euler(rotation);
            
            //-----------------------------бег---------------------------------------------------
            // позицию куда смещаемся сдвинуть вперед
            // !! обратились к найденному у игрока КОМПОНЕНТУ ригибоди
            // forward - направление
            // вторые скобки шоб плавающая точка не мешала умножать
            _rigidbody.MovePosition(position + transform.forward * (forwardspeed * Time.deltaTime));
            // -------------------- что было +       вектор       * скорость
        }



        // ------- ТРИГГЕР ------ без столкноваения -------------------------------------------------------------------
        // ------- для триггера (не пнет чела, но сработает) --- надо шоб был ригибади
        private void OnTriggerEnter(Collider other) // че за other? ещё нету его вроде
        {
            if (other.gameObject)
            {
                Finish();
                Dobezal?.Invoke(); // ! КУБ БОЛЬШЕ НЕ РАБОТАЕТ 
            }
        }


        // ----------- КОЛЛИЖН --------------стандартный метод ----- не 2D! (2D - OnCOllisionEnter2D) ------------------ 
        // -- если у ригибади есть дочерний коллайдер - тоже вызовет событие (доп коллайдер на меч, а ригибади у чувака)
        private void OnCollisionEnter(Collision collision) // возвращает объект, с котопым сколлизились
        {
            // правильнее всего делать проверку на наличие КОМПОНЕНТА на том, с чем столкнулись
            // компоненты только для детекта, там пусто
            
            // вернет TRUE если компонент удалось получить, FALSE если не удалось
            // тут не заводили ссылку а сразу получили компонент с объекта
            if (collision.gameObject.GetComponent<WallComponent>())
            {
                Died();
                OnDie?.Invoke(); 
            }
            
            
            if (collision.gameObject.GetComponent<WInCubeComponent>())
            {
                Finish();
               
            }
            
            
            Debug.Log("Collision Enter"); // всего есть три типа, оставаться и выйти из коллизии 
        }

        
        
        
        
        
        
        //------------------ методы в которых меняется анимация чувачка ------------------
        
        [ContextMenu("Died")] // это вылезет правой кнопкой из скрипта в инспекторе - для дебагов метода 
        //дабы проверить что метод работает
        private void Died()
        {
            _isActive = false;
            _animator.SetTrigger(Fall);
            
            OnDie?.Invoke();
            StartCoroutine(WaitTillHeFalls()) ;
        }

        private IEnumerator WaitTillHeFalls()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("Корутина сработала");
        }


        
        
        
        [ContextMenu("Dance")]
        private void Finish()
        {
            _isActive = false;
            _animator.SetTrigger(Dance);
            Dobezal?.Invoke();
        }


    }

}