using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChreaterController : MonoBehaviour
{
    [Header("Option")]
    [SerializeField] float _rotAngle;
    [SerializeField] float _speed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _jumpSpeed;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] KeyOthion _keyOthion = new KeyOthion();

    [Header("Connected Obhects")]
    [SerializeField] Transform _hitDamageZone;
    [SerializeField] GameObject _wepon;
    [SerializeField] GameObject _wepon_Hold;
    


    Rigidbody _rigidbody;
    CapsuleCollider _capsule;
    float _groundDistance; // 바닥 거리
    Vector3 _groundNormal;

    float _castRadius; // Sphere, Capsule 레이캐스트 반지름
    Vector3 CapsuleTopCenterPoint
        => new Vector3(transform.position.x, transform.position.y + _capsule.height - _capsule.radius, transform.position.z);
    Vector3 CapsuleBottomCenterPoint
        => new Vector3(transform.position.x, transform.position.y + _capsule.radius, transform.position.z);
    [Tooltip("지면으로 체크할 레이어 설정")]
    public LayerMask _groundLayerMask = -1;

    [Range(0.01f, 0.5f), Tooltip("전방 감지 거리")]
    public float _forwardCheckDistance = 0.1f;

    [Range(0.1f, 10.0f), Tooltip("지면 감지 거리")]
    public float _groundCheckDistance = 2.0f;

    [Range(0.0f, 0.1f), Tooltip("지면 인식 허용 거리")]
    public float _groundCheckThreshold = 0.01f;
    float _capsuleRadiusDiff;

    [Serializable] public class KeyOthion
    {
        public KeyCode moveForward = KeyCode.UpArrow;
        public KeyCode moveBackward = KeyCode.DownArrow;
        public KeyCode moveLeft = KeyCode.LeftArrow;
        public KeyCode moveRight = KeyCode.RightArrow;
        public KeyCode run = KeyCode.LeftShift;
        public KeyCode jump = KeyCode.C;
        public KeyCode attack = KeyCode.X;
        public KeyCode Skill1 = KeyCode.Q;
        public KeyCode Skill2 = KeyCode.W;
        public KeyCode Skill3 = KeyCode.E;
        public KeyCode Skill4 = KeyCode.R;
    } // 나중에 따로 스크립트로 빼기..


    Vector3 _moveDir;
    float _horizontal;
    float _vertical;
    bool _isMoving;
    bool _isGrounded;
    bool _isJumping;
    bool _isRun;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsule = GetComponent<CapsuleCollider>();
        _capsuleRadiusDiff = _capsule.radius - _castRadius + 0.05f;
    }
    private void Update()
    {
        InputKey();
    }
    private void FixedUpdate()
    {
        Move();
        Turn();
        CheckGround();
    }
    void InputKey()
    {
        _horizontal = 0; 
        _vertical = 0;
        if (Input.GetKey(_keyOthion.moveForward)) _vertical += 1.0f;
        if (Input.GetKey(_keyOthion.moveBackward)) _vertical -= 1.0f;
        if (Input.GetKey(_keyOthion.moveLeft)) _horizontal -= 1.0f;
        if (Input.GetKey(_keyOthion.moveRight)) _horizontal += 1.0f;
        
        _isMoving = _horizontal !=0 || _vertical !=0;
        _isRun = Input.GetKey(_keyOthion.run);
        
        if (Input.GetKeyDown(_keyOthion.jump)) Jump();


    }
    void Move()
    {       
        float speed;
        if (_isRun)
            speed = _runSpeed;
        else
            speed = _speed;
        _moveDir.Set(_horizontal, 0, _vertical);
        _moveDir = _moveDir.normalized * speed * Time.deltaTime;
        _moveDir = transform.TransformDirection(_moveDir);
        _rigidbody.MovePosition(transform.position + _moveDir);
    }
    void Turn()
    {
        if (!_isMoving) return;

        //transform.LookAt(transform.position +_moveDir);
        //transform.forward = Vector3.Lerp(transform.forward, _moveDir,_rotAngle *Time.deltaTime);
       // Quaternion newrot = Quaternion.LookRotation(_moveDir);
       // _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, newrot, _rotAngle * Time.deltaTime);
    }
    void Jump()
    {
        if (!_isGrounded)
            return;

        _rigidbody.AddForce(Vector3.up * _jumpSpeed, ForceMode.VelocityChange);
        
    }
    void CheckGround()
    {
        _groundDistance = float.MaxValue;
        _groundNormal = Vector3.up;

        bool cast = Physics.SphereCast(CapsuleBottomCenterPoint, _castRadius, Vector3.down, out var hit, _groundCheckDistance, _groundLayerMask, QueryTriggerInteraction.Ignore);

        _isGrounded = false;
        if (cast)
        {
            _groundNormal = hit.normal;
            _groundDistance = Mathf.Max(hit.distance - _capsuleRadiusDiff - _groundCheckThreshold, 0f);
            _isGrounded = _groundDistance <= 0.1f;
        }
    }

}
