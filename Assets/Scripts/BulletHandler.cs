using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private float calculatedSpeed;

    private Rigidbody2D _rb;
    private PlayerMovement player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerMovement>();

        calculatedSpeed = player.transform.localScale.x * bulletSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        _rb.linearVelocityX = calculatedSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
