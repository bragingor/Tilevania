using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed, jumpForce, climbSpeed;
    [SerializeField] private Vector2 deathKickedVelocity, bounceVelocity;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunPosition;

    [SerializeField] private AudioClip jumpSound, deathSound, bulletSound, bounceSound, levelExitSound, walkingSound;

    private Vector2 moveInput;
    private float startingGravity;

    private Rigidbody2D _rb;
    private Animator _anim;
    private CapsuleCollider2D _playerCapsuleCollider;
    private BoxCollider2D _playerBoxCollider;
    private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        _playerBoxCollider = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        startingGravity = _rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        ClimbUp();
        FlippingSprite();

        if (Keyboard.current[Key.Escape].isPressed)
        {
            Application.Quit();
        }
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            _audioSource.clip = bulletSound;
            _audioSource.Play();
            Instantiate(bullet, gunPosition.position, Quaternion.identity);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (Mathf.Abs(moveInput.x) > 0f)
        {
            _audioSource.clip = walkingSound;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }
    void OnJump(InputValue value)
    {
        if (!_playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            _rb.linearVelocityY = jumpForce;
            //_rb.AddForce(Vector2.up * jumpForce * 1000 * Time.deltaTime);
            _audioSource.loop = false;
            _audioSource.clip = jumpSound;
            _audioSource.Play();
        }
    }

    void Walk()
    {
        var moveVector = new Vector2(moveInput.x * walkSpeed, _rb.linearVelocityY);
        _rb.linearVelocity = moveVector;
        _anim.SetBool("isRunning", (Mathf.Abs(moveVector.x)) >= 1f);
    }
    void ClimbUp()
    {
        if (!_playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _anim.SetTrigger("isDying");
            _audioSource.loop = false;
            _audioSource.clip = deathSound;
            _audioSource.Play();
            _rb.linearVelocity = deathKickedVelocity;
            this.enabled = false;
        }
        else if (collision.gameObject.tag == "Bouncing")
        {
            _rb.linearVelocityY = bounceVelocity.y;
            _audioSource.loop = false;
            _audioSource.clip = bounceSound;
            _audioSource.Play();
        }
    }

    public void ProcessDeath()
    {
        FindFirstObjectByType<GameSession>().ProcessDeath();
    }
    IEnumerator NextLevel(float time)
    {
        yield return new WaitForSeconds(time);

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0); //lol
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            _audioSource.loop = false;
            _audioSource.clip = levelExitSound;
            _audioSource.Play();
            StartCoroutine(NextLevel(1f));
        }
    }
}
