using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveState : StateBehaviorBase<PlayerState>
{
    [Header("config")] 
    [FormerlySerializedAs("speed")] 
    [SerializeField] private float sprintSpeed = 1f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float jumpForce = 1f;
    private float _speed;

    [Header("sound")]
    [SerializeField] private SoundObject soundObjectPrefab;
    [SerializeField] private SoundObjectCreator soundObjectCreator;
    [SerializeField] private AudioSource footstep;

    private Camera _playerCamera;
    private CharacterController _cc;
    private IDisposable _soundStream;
    private Vector3 _inputAxis;

    private bool _isJumping = false;
    private Vector3 _acceleration = Vector3.zero;

    private Animator _animator;
    #region Unity

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerCamera = Camera.main;
    }

    #endregion
    
    public override PlayerState GetStateKey() => PlayerState.Move;

    public override void StateEnter()
    {
        base.StateEnter();

        _soundStream = Observable.Interval(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ =>
            {
                if (_inputAxis == Vector3.zero) return;
                if (Math.Abs(_speed - walkSpeed) < float.Epsilon) return;
                
                soundObjectCreator.Create(transform, soundObjectPrefab);
            });
    }

    public override void StateUpdate()
    {
        // 2d input
        _inputAxis = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        _inputAxis = Quaternion.Euler(0, _playerCamera.transform.eulerAngles.y, 0) * _inputAxis;

        // jump input
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            _acceleration = jumpForce * Vector3.up;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = walkSpeed;
        }
        else
        {
            _speed = sprintSpeed;
        }
        
        Debug.DrawRay(transform.position, _inputAxis, Color.blue);
        
        transform.LookAt(transform.position + _inputAxis);
        _cc.Move(_inputAxis * _speed * Time.deltaTime);
        
        // gravity
        if (_cc.isGrounded)
        {
            _isJumping = false;
            _acceleration = Vector3.zero;
        }
        else
        {
            _acceleration += 9.8f * Vector3.down * Time.deltaTime;
            _cc.Move(_acceleration * Time.deltaTime);
        }
        
        // animation
        _animator.SetFloat("Speed", _speed / sprintSpeed * _inputAxis.magnitude);
        
        if (Math.Abs(_speed - sprintSpeed) < float.Epsilon && _inputAxis.magnitude > 0f)
        {
            if (footstep.isPlaying) return;
            footstep.Play();
        }
        else
        {
            footstep.Stop();
        }
        
    }

    public override void StateExit()
    {
        base.StateExit();
        
        footstep.Stop();
        _soundStream.Dispose();
    }
}