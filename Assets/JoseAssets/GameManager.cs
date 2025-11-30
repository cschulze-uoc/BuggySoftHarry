using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerJose : MonoBehaviour
{
    public static GameManagerJose Instance;

    public GameObject gameOverPanel;

    public float winTime = 20f;
    private float timer = 0f;
    private bool gameEnded = false;

    private BroomController player;

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<BroomController>();
    }

    private void Update()
    {
        if (gameEnded) return;
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;

        if (timer >= winTime)
        {
            WinGame();
        }
    }

    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        FreezePlayer();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        // Cambiar texto a GAME OVER
        TextMeshProUGUI text = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = "GAME OVER";
    }

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        FreezePlayer();

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Cambiar texto a VICTORIA
        TextMeshProUGUI text = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = "�VICTORIA!";
    }

    private void FreezePlayer()
    {
        if (player != null)
        {
            player.isAlive = false;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }

    public void RestartMicrogame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}