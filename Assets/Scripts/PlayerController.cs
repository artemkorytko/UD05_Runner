using System;
using UnityEngine;

namespace Runner
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed = 5f;
        [SerializeField] private float _roadWidth = 5f;
        [SerializeField] private float _turnRotationAngle = 30f;
        [SerializeField] private float _lerpSpeed = 5f;
        [SerializeField] private Transform _model;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private InputHandler _inputHandler;
        private bool _isActive;
        
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Danse = Animator.StringToHash("Danse");

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    _animator.SetTrigger(Run);
                }
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            IsActive = true;
        }

        private void FixedUpdate()
        {
            if(!IsActive)
                return;
            
            Move(); // сначала повернуться потом идти !!!!!!
        }

        private void Move()
        {
            
            var xOffset = - _inputHandler.HorizontalAxis * _roadWidth ; // знак (-) делает отрицательное значение и при его отсутствии нажатие влево будет работать вправо
            var position = _rigidbody.position;
            position.x += xOffset;

            
            var rotation = _model.localRotation.eulerAngles;
            rotation.y = Mathf.LerpAngle(rotation.y, xOffset == 0 ? 0 : Mathf.Sign(xOffset) * _turnRotationAngle,
                _lerpSpeed * Time.deltaTime);
            _model.localRotation = Quaternion.Euler(rotation);  // поворачивается 
            
            
            _rigidbody.MovePosition(position + transform.forward * (_forwardSpeed * Time.deltaTime));  // идет 
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<WallComponent>())
                Died();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FinishComponent>())
                Finish();
        }

        [ContextMenu("Died")] // атрибут в инспекторе можно вызывать метод (ПКМ) по компаненту
        private void Died()
        {
            IsActive = false;
            _animator.SetTrigger(Fall);
        }
        
        [ContextMenu("Finish")]
        private void Finish()
        {
            IsActive = false;
            _animator.SetTrigger(Danse);
        }
    }
}