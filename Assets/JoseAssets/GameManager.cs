using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;   //  TEXTO DE PUNTUACIÓN

    [Header("Gameplay")]
    public float winTime = 20f;

    private float timer = 0f;
    private bool gameEnded = false;

    private int score = 0;              //  PUNTUACIÓN LOCAL
    private BroomController player;

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<BroomController>();

        // Inicializar score
        score = 0;
        if (scoreText != null)
            scoreText.text = "0";
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

    // ---------------- PUNTUACIÓN ----------------
    public void AddScore(int amount)
    {
        score += amount;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    // ---------------- FIN DE JUEGO ----------------
    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        FreezePlayer();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

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

        TextMeshProUGUI text = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = "¡VICTORIA!";
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

    // ---------------- REINICIO ----------------
    public void RestartMicrogame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}