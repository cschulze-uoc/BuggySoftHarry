using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;   // <- NUEVO INPUT SYSTEM

public class ARDementorGame : MonoBehaviour
{
    public static ARDementorGame Instance { get; private set; }

    [Header("Referencias")]
    public Camera arCamera;
    public GameObject dementorPrefab;
    public GameObject spellPrefab;

    [Header("UI HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI messageText;

    [Header("UI Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalText;

    [Header("Ajustes de juego")]
    public float dementorDistance = 3f;
    public float spellSpawnDistance = 0.3f;
    public int initialLives = 3;
    public float gameDurationSeconds = 180f;

    [Header("Dificultad (vida y velocidad)")]
    public int baseDementorHealth = 1;
    public float baseDementorSpeed = 0.4f;

    [Header("Dificultad (spawns)")]
    public float spawnIntervalStart = 3f;
    public float spawnIntervalEnd = 0.8f;
    public int maxConcurrentStart = 2;
    public int maxConcurrentEnd = 8;
    public float timeToMaxDifficulty = 120f;

    [Header("Escala / Aparición en pantalla")]
    public float dementorScale = 0.7f;
    [Range(0f, 1f)] public float minViewportX = 0.1f;
    [Range(0f, 1f)] public float maxViewportX = 0.9f;
    [Range(0f, 1f)] public float minViewportY = 0.2f;
    [Range(0f, 1f)] public float maxViewportY = 0.9f;

    [Header("Sonidos")]
    public AudioClip sfxCast;
    public AudioClip sfxDementorAppear;
    public AudioClip sfxDementorDie;
    public AudioClip sfxPlayerHit;
    public AudioClip sfxSpellImpact;

    [Header("Orientación modelo 3D")]
    public Vector3 dementorModelEulerOffset = new Vector3(-90f, 180f, 270f);

    [Header("Golpe cargado")]
    public float chargeThreshold = 0.5f;   // seg para que sea cargado
    public int normalDamage = 1;
    public int chargedDamage = 2;

    [Header("Golpe cargado UI")]
    public GameObject chargeIcon;          // objeto Image cerca de la varita
    public Image chargeIconImage;          // componente Image

    [Header("Efecto de brillo al cargar")]
    public Image chargeGlowImage;

    // ---- Estado interno ----
    int score;
    int lives;
    float timeLeft;
    int dementorsDefeated;
    float spawnTimer;
    bool isGameOver;

    bool isTouching;
    float touchStartTime;
    float chargeProgress;

    readonly List<DementorController> aliveDementors = new List<DementorController>();

    // ==========================
    // CICLO DE VIDA
    // ==========================
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        lives = initialLives;
        timeLeft = gameDurationSeconds;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (chargeIcon != null)
            chargeIcon.SetActive(false);

        if (chargeGlowImage != null)
        {
            Color c = chargeGlowImage.color;
            c.a = 0f;
            chargeGlowImage.color = c;
        }

        UpdateScoreUI();
        UpdateLivesUI();
        UpdateTimeUI();

        ShowMessage("Busca dementores y TOCA SOBRE ELLOS para lanzar el Patronus.", 4f);

        for (int i = 0; i < maxConcurrentStart; i++)
            SpawnDementor();
    }

    void Update()
    {
        if (isGameOver) return;

        // Tiempo
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            GameOver("Tiempo agotado");
        }
        UpdateTimeUI();

        // Spawns
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            int maxCurrent = GetCurrentMaxConcurrent();
            if (aliveDementors.Count < maxCurrent)
                SpawnDementor();

            spawnTimer = GetCurrentSpawnInterval();
        }

        // ==========================
        // INPUT TÁCTIL / RATÓN (NEW INPUT SYSTEM)
        // ==========================

        // 1) Táctil (móvil)
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                StartCharging();
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                Vector2 pos = touch.position.ReadValue();
                StopChargingAndShoot(pos);
            }
        }
        // 2) Ratón (editor / PC)
        else if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartCharging();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Vector2 pos = Mouse.current.position.ReadValue();
                StopChargingAndShoot(pos);
            }
        }

        // ==========================
        // ANIMACIÓN ICONO CARGA
        // ==========================
        if (isTouching && chargeIcon != null)
        {
            chargeProgress = Mathf.Clamp01((Time.time - touchStartTime) / chargeThreshold);
            if (chargeIconImage != null)
                chargeIconImage.fillAmount = chargeProgress;

            float scale = Mathf.Lerp(0.4f, 1.2f, chargeProgress);
            chargeIcon.transform.localScale = new Vector3(scale, scale, 1f);

            if (chargeIconImage != null)
            {
                Color c = chargeIconImage.color;
                c.a = Mathf.Lerp(0.2f, 0.9f, chargeProgress);
                chargeIconImage.color = c;
            }

            if (chargeGlowImage != null)
            {
                // Empieza a brillar a partir del 70% de carga
                float glow = Mathf.Clamp01((chargeProgress - 0.7f) / 0.3f); // 0 cuando <0.7, 1 cuando >=1
                Color gc = chargeGlowImage.color;
                gc.a = glow * 0.03f;
                chargeGlowImage.color = gc;
            }

            if (chargeProgress >= 1f)
            {
                chargeIcon.transform.Rotate(Vector3.forward * 120f * Time.deltaTime);
            }
        }

        if (!isTouching && chargeGlowImage != null)
        {
            Color gc = chargeGlowImage.color;
            gc.a = Mathf.MoveTowards(gc.a, 0f, Time.deltaTime * 2f); // se desvanece rápido
            chargeGlowImage.color = gc;
        }
    }

    void StartCharging()
    {
        isTouching = true;
        touchStartTime = Time.time;
        chargeProgress = 0f;

        if (chargeIcon != null)
        {
            chargeIcon.SetActive(true);
            chargeIcon.transform.localScale = Vector3.zero;

            if (chargeIconImage != null)
            {
                Color c = chargeIconImage.color;
                c.a = 0.2f;
                chargeIconImage.color = c;
            }
        }
    }

    void StopChargingAndShoot(Vector2 screenPos)
    {
        if (!isTouching) return;

        isTouching = false;

        float held = Time.time - touchStartTime;
        bool charged = held >= chargeThreshold;

        HandleScreenTap(screenPos, charged);

        if (chargeIcon != null)
            chargeIcon.SetActive(false);
    }

    // ==========================
    // DIFICULTAD
    // ==========================
    float GetDifficultyFactor()
    {
        float elapsed = gameDurationSeconds - timeLeft;
        return Mathf.Clamp01(elapsed / timeToMaxDifficulty);
    }

    float GetCurrentSpawnInterval()
    {
        float t = GetDifficultyFactor();
        return Mathf.Lerp(spawnIntervalStart, spawnIntervalEnd, t);
    }

    int GetCurrentMaxConcurrent()
    {
        float t = GetDifficultyFactor();
        return Mathf.RoundToInt(Mathf.Lerp(maxConcurrentStart, maxConcurrentEnd, t));
    }

    int GetCurrentHealth()
    {
        float t = GetDifficultyFactor();
        return baseDementorHealth + Mathf.FloorToInt(t * 3f); // 1..4 vidas
    }

    float GetCurrentSpeed()
    {
        float t = GetDifficultyFactor();
        return baseDementorSpeed + t * 0.6f;
    }

    // ==========================
    // ATAQUE DEL JUGADOR
    // ==========================
    void HandleScreenTap(Vector2 screenPos, bool charged)
    {
        if (arCamera == null || spellPrefab == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20f))
        {
            DementorController dementor = hit.collider.GetComponentInParent<DementorController>();
            if (dementor != null)
            {
                Vector3 spawnPos = arCamera.transform.position + arCamera.transform.forward * spellSpawnDistance;
                Vector3 dir = (hit.point - spawnPos).normalized;
                Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

                GameObject spellObj = Instantiate(spellPrefab, spawnPos, rot);

                SpellController spell = spellObj.GetComponent<SpellController>();
                if (spell != null)
                {
                    spell.damage = charged ? chargedDamage : normalDamage;
                }

                if (charged)
                {
                    PlaySound(sfxCast);
                }
            }
        }
    }

    // ==========================
    // SPAWN DEMENTORES
    // ==========================
    void SpawnDementor()
    {
        if (arCamera == null || dementorPrefab == null) return;

        Vector3 cameraPos = arCamera.transform.position;

        float vx = Random.Range(minViewportX, maxViewportX);
        float vy = Random.Range(minViewportY, maxViewportY);
        Vector3 vp = new Vector3(vx, vy, dementorDistance);
        Vector3 worldPos = arCamera.ViewportToWorldPoint(vp);

        GameObject obj = Instantiate(dementorPrefab, worldPos, Quaternion.identity);
        obj.transform.localScale = Vector3.one * dementorScale;

        Vector3 lookDir = (cameraPos - obj.transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(lookDir, Vector3.up);
        Quaternion baseRot = Quaternion.Euler(dementorModelEulerOffset);
        obj.transform.rotation = lookRot * baseRot;

        PlaySound(sfxDementorAppear);

        DementorController dementor = obj.GetComponent<DementorController>();
        if (dementor != null)
        {
            int health = GetCurrentHealth();
            float speed = GetCurrentSpeed();

            dementor.Init(this, arCamera.transform, health, speed);
            aliveDementors.Add(dementor);
        }
    }

    // ==========================
    // UI / ESTADO
    // ==========================
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntuación: " + score;
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
    }

    void UpdateTimeUI()
    {
        if (timeText == null) return;

        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        int secs = seconds % 60;

        timeText.text = $"Tiempo: {minutes:00}:{secs:00}";
    }

    public void OnDementorKilled(DementorController dementor)
    {
        if (isGameOver) return;

        score += 100;
        dementorsDefeated++;
        aliveDementors.Remove(dementor);
        UpdateScoreUI();

        PlaySound(sfxDementorDie);
        ShowMessage("¡Dementor derrotado!", 1.5f);
    }

    public void PlayerHit(DementorController dementor)
    {
        if (isGameOver) return;

        aliveDementors.Remove(dementor);

        lives--;
        if (lives < 0) lives = 0;
        UpdateLivesUI();

        PlaySound(sfxPlayerHit);
        Handheld.Vibrate();
        ShowMessage("¡Te ha alcanzado un dementor!", 1.5f);

        if (lives <= 0)
        {
            GameOver("Te has quedado sin vidas");
        }
    }

    void GameOver(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;

        ShowMessage("", 0f);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalText != null)
            finalText.text = $"GAME OVER:\n{reason}\nPuntuación: {score}";
    }

    void ShowMessage(string text, float duration)
    {
        if (messageText == null) return;

        messageText.text = text;
        CancelInvoke(nameof(ClearMessage));

        if (duration > 0f)
            Invoke(nameof(ClearMessage), duration);
    }

    void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    public void PlaySpellImpact()
    {
        PlaySound(sfxSpellImpact);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null || arCamera == null) return;
        AudioSource.PlayClipAtPoint(clip, arCamera.transform.position);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    // ==========================
    // BOTONES UI (NEW INPUT SYSTEM NO CAMBIA ESTO)
    // ==========================
    public void OnRetryButton()
    {

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void OnQuitButton()
    {

        Application.Quit();
    }
}
