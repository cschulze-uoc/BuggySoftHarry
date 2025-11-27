using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HorrocruxGameManager : MonoBehaviour
{
    public static HorrocruxGameManager Instance;
    private int lastIndex = -1;
    private int score = 0;

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
        currentTimeLimit = startTimeLimit;
        HUDController.Instance.UpdateScore(score);
        SpawnHorrocrux();
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        HUDController.Instance.UpdateTimer(countdown);
        if (countdown <= 0f)
            SceneManager.LoadScene(0); // volver al menú
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
            do
            {
                newIndex = Random.Range(0, spawnPoints.Count);
            }
            while (newIndex == lastIndex);
        }

        lastIndex = newIndex;

        // Obtener punto final
        Transform point = spawnPoints[newIndex];

        horrocrux.position = point.position;
        horrocrux.rotation = point.rotation;

        countdown = currentTimeLimit;

        currentTimeLimit -= timeDecreasePerHorrocrux;
        if (currentTimeLimit < 2f) currentTimeLimit = 2f;
    }

    // llamado por HorrocruxController
    public void OnHorrocruxCollected()
    {
        score++;
        HUDController.Instance.UpdateScore(score);
        SpawnHorrocrux();
    }
}