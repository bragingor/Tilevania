using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private AudioClip _coinSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_coinSound, Camera.main.transform.position);
            FindFirstObjectByType<GameSession>().AddScore();
            Destroy(gameObject);
        }
    }
}
