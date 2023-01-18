using System;
using Runner.Configs;
using UnityEngine;

namespace Runner
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private Transform model; 
        
        private float _forwardSpeed = 5f;
        private float _sideSpeed = 5f; 
        private float _roadWidth = 5f;  // переменная для смещения в зависимости от ширины дороги (условно это ширина дороги / на 2)
        private float _turnRotationAngle = 40f; // максимальный угол поворота 
        private float _lerpSpeed = 4f; 
        

        private Rigidbody _rigidbody;
        private Animator _animator;
        private InputHandler _inputHandler;
        private bool _isActive; // флаг который при победе/смерти будет опусткаться. (false) - FixedUpdate() будет return т.е метод Move не будет работать
        private int _countCoits;
        
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Dance = Animator.StringToHash("Dance");
        

        public event Action OnWin;
        public event Action OnDead;
        public event Action<int> OnCoint;
        
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

            SetSettings();
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
            // Debug.Log(_inputHandler.HorizontalAxis);
            
            var xOffset = - _inputHandler.HorizontalAxis * _sideSpeed; // знак "-" при его отсутствии нажатие влево будет работать вправо(xOffset - смещение) 
            
            // поворачивается //
            var rotation = model.localRotation.eulerAngles; // запись углов модели
            rotation.y = Mathf.LerpAngle(rotation.y, _inputHandler._isHold  ?  Mathf.Sign(xOffset) * _turnRotationAngle : 0,  // решить проблему в повороте!!!!!!!сюда смотри!!!!!!!
                _lerpSpeed * Time.deltaTime); // плавное(LerpAngle) изменение поворота по оси Y(лево/право) в зависимости от xOffset(который определяет навровление поворота)
            // Mathf.Sign(xOffset) возвращает знак + или - нашего xOffset.
            
            
             model.localRotation = Quaternion.Euler(rotation);  // поворачивается переводим угол eulerAngles в Quaternion

             // идет //
            var position = _rigidbody.position; // запись первоначальной позиции 
            position.x += xOffset; // изменение координаты X в напровление движения игрока - передается в _rigidbody.MovePosition();
            position.x = Mathf.Clamp(position.x, -_roadWidth * 0.5f, _roadWidth * 0.5f); // ограничение движения игрока по оси X в зависимости от ширины дороги
            _rigidbody.MovePosition(position + transform.forward * (_forwardSpeed * Time.deltaTime));  // идет (к position вперед(transform.forward (forward(x=0, y=0, z=1)))
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

            if (other.gameObject.TryGetComponent(out CoinComponent coinComponent))
            {
                AddCoits();
                coinComponent.gameObject.SetActive(false);
            }
        }
        
        private void SetSettings()
        {
            _forwardSpeed = playerConfig.ForwardSpeed;
            _sideSpeed = playerConfig.SideSpeed;
            _roadWidth = playerConfig.RoadWidth;
            _turnRotationAngle = playerConfig.TurnRotationAngle;
            _lerpSpeed = playerConfig.LerpSpeed;
        }

        private void AddCoits()
        {
            _countCoits++;
            OnCoint?.Invoke(_countCoits);
        }
        
        private void Died()
        {
            IsActive = false;
            _animator.SetTrigger(Fall);
            OnDead?.Invoke();
        }
        
        private void Finish()
        {
            IsActive = false;
            _animator.SetTrigger(Dance);
            OnWin?.Invoke();
        }
    }
}