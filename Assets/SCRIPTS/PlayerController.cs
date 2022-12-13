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
        [SerializeField] private float forwardspeed; //скорость для Move
        [SerializeField] private float roadWidth;
        [SerializeField] private float turnRotationAngle = 30f;
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Transform model;

        private Rigidbody _rigidbody; // ссылка на ригибоди ДЛЯ ДВИЖЕНИЯ!
        private Animator _animator; //дочерний от вьюшки
        private InputHandler _inputHandler;

        public event Action Dobezal; 

        // флаг про то что изменилось состояние активности
        // приват можно не писать, они и так по умолчанию...
        public bool _isActive;
        
        // эти штуки райдер пишет автоматически поругавшись на "" в коде
        // строковые значения надо хешировать, также чтобы каждый раз оно не хешировало пока выполняет колд
        private static readonly int Run = Animator.StringToHash("Run"); // на старте хешируем
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int Fall = Animator.StringToHash("Fall");

        // чтобы понимать, что этот флаг меняется - нужен геттер и сеттер :/ - это я хреново понимаю
        public bool IsActive
        {
            get => _isActive; //возвращает
            
            set //делает логику
            {
                _isActive = value;

                if (_isActive) //true
                {
                    _animator.SetTrigger(Run); //alt enter - hash <----- передали название триггера (в аниматоре +)
                }
            }
        }

        private void Start()
        {
            IsActive = true; // че за
        }

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        
        // ----------- чтобы двигаться --------------------------------------------------------------------------
        private void FixedUpdate() // не зависит от частоты кадров, вызывается до update
        {
            if (!_isActive)
                return;
            Move(); 
        }


        void Move()
        {
            var xOffset = -_inputHandler.HorizontalAxis * roadWidth; // минус - это то же что домножить на -1

            var position = _rigidbody.position; // получаем позицию у ригибоди чувачка
            position.x += xOffset;
            
            // ПРАВИЛО всегда сначала повороты потом движение!!!!!
            // берем углы модели
            var rotation = model.localRotation.eulerAngles;
            
            // ноль - положительное число
            rotation.y = Mathf.LerpAngle(rotation.y, xOffset == 0? 0 :Mathf.Sign(xOffset) * turnRotationAngle, lerpSpeed * Time.deltaTime);
            model.localRotation = Quaternion.Euler(rotation);
            
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
            }
            
            
            if (collision.gameObject.GetComponent<WInCubeComponent>())
            {
                Finish();
                Dobezal?.Invoke();
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
        }





        [ContextMenu("Dance")]
        private void Finish()
        {
            _isActive = false;
            _animator.SetTrigger(Dance);
        }

        
        
    }

}