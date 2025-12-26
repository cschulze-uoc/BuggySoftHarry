using UnityEngine;
using TMPro;
using System.Collections;

public class QDGameManager : MonoBehaviour
{
    public static QDGameManager Instance;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public GameObject tapToStartText;

    [Header("Gameplay")]
    public float winTime = 20f;
    public float endDelay = 1.2f;   //  tiempo antes de pasar al siguiente juego

    [Header("Score Animation")]
    public float scorePopScale = 1.3f;
    public float scorePopDuration = 0.15f;

    [Header("Audio")]
    public AudioSource scoreAudio;

    private float timer = 0f;
    private bool gameEnded = false;
    private bool gameStarted = false;

    private int score = 0;
    private BroomController player;
    private Vector3 scoreOriginalScale;

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<BroomController>();

        score = 0;
        if (scoreText != null)
        {
            scoreText.text = "0";
            scoreOriginalScale = scoreText.transform.localScale;
        }

        // 🔒 Esperar TAP para empezar
        Time.timeScale = 0f;
        gameStarted = false;

        if (tapToStartText != null)
            tapToStartText.SetActive(true);
    }

    private void Update()
    {
        // ---------- TAP PARA EMPEZAR ----------
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetMouseButtonDown(0) ||
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                StartGame();
            }
            return;
        }

        // ---------- JUEGO NORMAL ----------
        if (gameEnded) return;

        timer += Time.deltaTime;

        if (timer >= winTime)
        {
            WinGame();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;

        if (tapToStartText != null)
            tapToStartText.SetActive(false);
    }

    // ---------------- PUNTUACIÓN ----------------
    public void AddScore(int amount)
    {
        score += amount;

        if (scoreText != null)
        {
            scoreText.text = score.ToString();
            StopCoroutine("ScorePop");
            StartCoroutine("ScorePop");
        }

        if (scoreAudio != null)
            scoreAudio.Play();
    }

    private IEnumerator ScorePop()
    {
        float t = 0f;
        Vector3 targetScale = scoreOriginalScale * scorePopScale;

        while (t < scorePopDuration)
        {
            t += Time.unscaledDeltaTime;
            scoreText.transform.localScale =
                Vector3.Lerp(scoreOriginalScale, targetScale, t / scorePopDuration);
            yield return null;
        }

        t = 0f;

        while (t < scorePopDuration)
        {
            t += Time.unscaledDeltaTime;
            scoreText.transform.localScale =
                Vector3.Lerp(targetScale, scoreOriginalScale, t / scorePopDuration);
            yield return null;
        }

        scoreText.transform.localScale = scoreOriginalScale;
    }

    // ---------------- FIN DE JUEGO ----------------
    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        FreezePlayer();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        TextMeshProUGUI text = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = "GAME OVER";

        StartCoroutine(EndAndGoNext());
    }

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        FreezePlayer();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        TextMeshProUGUI text = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = "¡VICTORIA!";

        StartCoroutine(EndAndGoNext());
    }

    private IEnumerator EndAndGoNext()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(endDelay);

        // ➕ sumar puntuación al global
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.totalScore += score;
            GlobalGameManager.Instance.GoToNextMinigame();
        }
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
}