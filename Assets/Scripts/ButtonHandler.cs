using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale *= 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
