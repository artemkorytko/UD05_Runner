using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Runner;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runner
{


    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float forwardspeed;
        [SerializeField] private float roadWidth;
        [SerializeField] private float turnRotationAngle = 30f;
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Transform model;

        private Rigidbody _rigidbody;
        private Animator _animator; //дочерний от вьюшки
        private InputHandler _inputHandler;

        public event Action Dobezal; 

        // флаг про то что изменилось состояние

        // привате можно не писать, они и так по умолчанию...
        public bool _isActive;
        private static readonly int Run = Animator.StringToHash("Run"); //на старте хешируем
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int Fall = Animator.StringToHash("Fall");

        public bool IsActive
        {
            get => _isActive;
            
            set
            {
                _isActive = value;

                if (_isActive) //true
                {
                    _animator.SetTrigger(Run); //alt enter - hash
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

        // гдето тут ретурн дописать!!!!!

        private void FixedUpdate()
        {
            if (!_isActive)
                return;
            Move();
        }


        void Move()
        {
            var xOffset = -_inputHandler.HorizontalAxis * roadWidth; // минус - это то же что домножить на -1

            var position = _rigidbody.position;
            position.x += xOffset;
            
            // всегда сначала повороты потом движение!!!!!
            //берем углы модели
            var rotation = model.localRotation.eulerAngles;
            
            // ноль - положительное число
            rotation.y = Mathf.LerpAngle(rotation.y, xOffset == 0? 0 :Mathf.Sign(xOffset) * turnRotationAngle, lerpSpeed * Time.deltaTime);
            model.localRotation = Quaternion.Euler(rotation);
            
            // forward - направление
            // вторые скобки шоб плавающая точка не мешала умножать
            _rigidbody.MovePosition(position + transform.forward * (forwardspeed * Time.deltaTime));
        }




        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject)
            {
                Finish();
            }
        }


        // ----------- стандартный метод от коллижен 
        // ------ а вот для триггера (не пнет но сработает)--- надо шоб был ригибади
        // -- если у ригибади есть дочерний коллайдер - тоже вызовет событие (доп коллайдер на меч, а ригибади у чувака)
        private void OnCollisionEnter(Collision collision) // возвращ то с кем сколлизились
        {
            // правильно делать проверку на наличие компонента

            // в больших пишут всегда var
            // вернет true если компонент удалось получить, фалс если не удалось
            // дописать одно
            if (collision.gameObject.GetComponent<WallComponent>())
            {
                Died();
            }
            
            
            if (collision.gameObject.GetComponent<WInCubeComponent>())
            {
                Finish();
                Dobezal?.Invoke(); 

            }
            

            Debug.Log("Collision Enter"); // еще три есть
        }

        
        
        
        
        
        
        
        
        [ContextMenu("Died")] // это вылезет правой кнопкой из скрипта в инспекторе
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