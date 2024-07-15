using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody2D _playerRb;
    private float _speedMovement = 7f;
    private float _horizontal;
    private Vector2 _moveVector;

    [Header("Jump")] 
    private float _jumpForce = 5f;

    [Header("Grounded")] 
    [SerializeField] private bool _isGrounded;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    
    private SpriteRenderer _playerSprite;

    private Animator _playerAnim;
    private State state;
    public IdleState idleState;
    public RunState runState;
    public JumpState jumpState;

    public Animator PlayerAnim { get => _playerAnim; }
    public bool IsGrounded { get => _isGrounded; }
    public Vector2 MoveVector { get => _moveVector; }

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        state = idleState;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        _isGrounded = CheckGround();
        FlipPlayer();
        if (state.isComplete)
        {
            SelectState();
        }
        state.Do();
    }

    private void SelectState()
    {
        if (_isGrounded)
        {
            if(_moveVector == Vector2.zero)
            {
                state = idleState;
            }
            else
            {
                state = runState;
            }
        }
        else
        {
            state = jumpState;
        }
        state.Enter();
    }

    private void MovePlayer()
    {
        _moveVector = new Vector2(_horizontal * _speedMovement, _playerRb.velocity.y);
        _playerRb.velocity = _moveVector;
    }
    
    public void Movement(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;
    }

    private void FlipPlayer()
    {
        if (_moveVector.x < 0)
        {
            _playerSprite.flipX = true;
        }
        else if (_moveVector.x > 0)
        {
            _playerSprite.flipX = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {          
        if (context.performed && _isGrounded)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpForce);
        }
    }

    private bool CheckGround()
    {
        if (Physics2D.Raycast(_groundCheck.position, Vector3.down, 0.2f, _groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        

}
