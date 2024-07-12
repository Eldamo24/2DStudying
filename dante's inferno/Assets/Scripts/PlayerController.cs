using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        _isGrounded = CheckGround();
        AnimationsState();
        FlipPlayer();
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

    private void AnimationsState()
    {
        if (!_isGrounded)
        {
            _playerAnim.SetBool("Jumping", true);
        }
        else
        {
            _playerAnim.SetBool("Jumping", false);
            if (_moveVector != Vector2.zero)
            {
                _playerAnim.SetBool("Running", true);
            }

            if (_moveVector == Vector2.zero)
            {
                _playerAnim.SetBool("Running", false);
            }
        }
    }
        

}
