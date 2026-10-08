using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed, jumpForce, climbSpeed;

    private Vector2 moveInput;
    private float startingGravity;

    private Rigidbody2D _rb;
    private Animator _anim;
    private CapsuleCollider2D _playerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _playerCollider = GetComponent<CapsuleCollider2D>();

        startingGravity = _rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        ClimbUp();
        FlippingSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!_playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            _rb.AddForce(Vector2.up * jumpForce * 1000 * Time.deltaTime);
        }
    }

    void Walk()
    {
        var moveVector = new Vector2(moveInput.x * walkSpeed * 100 * Time.deltaTime, _rb.linearVelocityY);
        _rb.linearVelocity = moveVector;
        _anim.SetBool("isRunning", (Mathf.Abs(moveVector.x)) >= 1f);
    }
    void ClimbUp()
    {
        if (!_playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        { 
            _rb.gravityScale = startingGravity;
            _anim.SetBool("isClimbing", false);
            return; 
        }
        _rb.gravityScale *= 0f;
        Vector2 climbingVector = new Vector2(_rb.linearVelocityX, moveInput.y * climbSpeed);
        _anim.SetBool("isClimbing", true);

        _rb.linearVelocityY = climbingVector.y;
    }

    void FlippingSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rb.linearVelocityX) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rb.linearVelocityX), 1f);
        }
    }
}
