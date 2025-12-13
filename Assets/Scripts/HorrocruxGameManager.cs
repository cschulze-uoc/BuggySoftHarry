using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HorrocruxGameManager : MonoBehaviour
{
    public static HorrocruxGameManager Instance;

    private int lastIndex = -1;

    // Puntuación LOCAL de este minijuego
    private int score = 0;

    // Puntuación global con la que entramos a este minijuego
    private int baseGlobalScore = 0;

    private bool primera = true;
    private bool isGameOver = false;

    [Header("Settings")]
    public Transform horrocrux;               // El único horrocrux
    public List<Transform> spawnPoints;       // Lista de puntos posibles
    public float startTimeLimit = 12f;
    public float timeDecreasePerHorrocrux = 1.5f;

    private float currentTimeLimit;
    private float countdown;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 1) Guardar la puntuación global con la que ENTRAMOS al minijuego
        if (GlobalGameManager.Instance != null)
            baseGlobalScore = GlobalGameManager.Instance.totalScore;
        else
            baseGlobalScore = 0;

        // 2) La puntuación LOCAL de este minijuego empieza en 0
        score = 0;

        // 3) Configurar el tiempo
        currentTimeLimit = startTimeLimit;
        countdown = currentTimeLimit;

        // 4) Mostrar en el HUD: global previa + local
        if (HUDController.Instance != null)
            HUDController.Instance.UpdateScore(baseGlobalScore + score);

        SpawnHorrocrux();
    }

    void Update()
    {
        if (isGameOver) return;

        countdown -= Time.deltaTime;
        if (HUDController.Instance != null)
            HUDController.Instance.UpdateTimer(countdown);

        if (countdown <= 0f)
        {
            // Fin del minijuego VR
            EndGame("Tiempo agotado");
        }
    }

    void EndGame(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;

        // Fijar puntuación final en el GLOBAL: base + local
        int finalScore = baseGlobalScore + score;

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.totalScore = finalScore;
            GlobalGameManager.Instance.GoToNextMinigame();
        }
        else
        {
            // Si se está jugando fuera de campaña, volver al menú por índice 0
            SceneManager.LoadScene(0);
        }
    }

    void SpawnHorrocrux()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("ERROR: No hay spawnPoints asignados.");
            return;
        }

        int newIndex;

        // Elegir un índice distinto al último
        if (spawnPoints.Count == 1)
        {
            newIndex = 0;   // si solo hay uno, no hay opción
        }
        else
        {
            if (primera)
            {
                newIndex = 8;
                primera = false;
            }
            else
            {
                do
                {
                    newIndex = Random.Range(0, spawnPoints.Count);
                }
                while (newIndex == lastIndex);
            }
        }

        lastIndex = newIndex;

        // Animación desaparecer
        var hdDesaparecer = horrocrux.GetComponent<HorrocruxAppearDisappear>();
        if (hdDesaparecer != null)
            hdDesaparecer.PlayDisappear();

        // Obtener punto final
        Transform point = spawnPoints[newIndex];

        horrocrux.position = point.position;
        horrocrux.rotation = point.rotation;

        // Animación aparecer
        var hdAparecer = horrocrux.GetComponent<HorrocruxAppearDisappear>();
        if (hdAparecer != null)
            hdAparecer.PlayAppear();

        countdown = currentTimeLimit;

        currentTimeLimit -= timeDecreasePerHorrocrux;
        if (currentTimeLimit < 2f) currentTimeLimit = 2f;
    }

    // llamado por HorrocruxController cuando lo pillas a tiempo
    public void OnHorrocruxCollected()
    {
        if (isGameOver) return;

        score++;

        if (HUDController.Instance != null)
            HUDController.Instance.UpdateScore(baseGlobalScore + score);

        SpawnHorrocrux();
    }

    // llamado por HorrocruxController cuando miras a la serpiente
    public void OnSnakeViewed()
    {
        if (isGameOver) return;

        score--;

        if (HUDController.Instance != null)
            HUDController.Instance.UpdateScore(baseGlobalScore + score);
    }
}
