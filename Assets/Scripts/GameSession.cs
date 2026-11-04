using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private TextMeshProUGUI scoreText, livesText;

    private int score;
    private void Awake()
    {
        int numberOfInstances = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        if (numberOfInstances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        scoreText.text = "0";
        livesText.text = playerHealth.ToString();
    }

    public void ProcessDeath()
    {
        if (playerHealth > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGame();
        }
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    private void TakeLife()
    {
        playerHealth--;
        livesText.text = playerHealth.ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
