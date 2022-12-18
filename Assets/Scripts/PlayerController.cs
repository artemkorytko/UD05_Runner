using System;
using Runner;
using UnityEngine;

public class PlayerController : MonoBehaviour
    { 
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float sideSpeed = 10f;
        [SerializeField] private float roadWidth = 6f;
        [SerializeField] private float turnRotationAngle = 30f;
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Transform model;
        
        private Rigidbody _rigidbody;
        private Animator _animator;
        private InputHandler _inputHandler;

        private bool _isActive;
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Dance = Animator.StringToHash("Dance");

        public event Action OnWin;
        public event Action OnDead;

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
            _animator = GetComponentInChildren<Animator>();
            _inputHandler = GetComponent<InputHandler>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if (!IsActive)
                return;
            Move();
        }
        
        private void Move()
        {
            var xOffset = -_inputHandler.HorizontalAxis * sideSpeed;
            var position = _rigidbody.position;
            position.x += xOffset;
            position.x = Mathf.Clamp(position.x, -roadWidth * 0.5f, roadWidth * 0.5f);

            var rotation = model.localRotation.eulerAngles;
            rotation.y = Mathf.LerpAngle(rotation.y, xOffset == 0 ? 0 : Mathf.Sign(xOffset) * turnRotationAngle, lerpSpeed * Time.deltaTime);
            model.localRotation = Quaternion.Euler(rotation);

            _rigidbody.MovePosition(position + transform.forward * (forwardSpeed * Time.deltaTime));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<WallComponent>())
            {
                Died();
                OnDead?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FinishComponent>())
            {
                Finish();
                OnWin?.Invoke();
            }
        }
        
        [ContextMenu("Died")]
        private void Died()
        {
            IsActive = false;
            _animator.SetTrigger(Fall);
        }

        [ContextMenu("Finish")]
        private void Finish()
        {
            IsActive = false;
            _animator.SetTrigger(Dance);
        }
    }
