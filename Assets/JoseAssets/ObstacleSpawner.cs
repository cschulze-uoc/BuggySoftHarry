using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject goalPrefab;          //  portería

    [Header("Spawn normal")]
    public float spawnInterval = 1.5f;

    [Header("Altura obstáculos")]
    public float minHeight = 1.0f;
    public float maxHeight = 5.0f;

    [Header("Posición")]
    public float groundY = -3.7f;

    [Header("Goal")]
    public float goalSpawnTime = 55f;       //  segundo en que aparece la portería

    private float timer;
    private float levelTimer;               //  tiempo total de la partida
    private bool goalSpawned = false;        //  para que salga solo una vez

    void Update()
    {
        if (Time.timeScale == 0f) return; // NO SPAWNEAR si el juego terminó

        timer += Time.deltaTime;
        levelTimer += Time.deltaTime;

        // ---- Obstáculos normales ----
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }

        // ---- Portería (solo una vez) ----
        if (!goalSpawned && goalPrefab != null && levelTimer >= goalSpawnTime)
        {
            goalSpawned = true;
            SpawnGoal();
        }
    }

    void SpawnObstacle()
    {
        float height = Random.Range(minHeight, maxHeight);

        GameObject newObstacle = Instantiate(obstaclePrefab);

        newObstacle.transform.position = new Vector3(
            transform.position.x,
            groundY,
            0f
        );

        newObstacle.transform.localScale = new Vector3(
            1f,
            height,
            1f
        );
    }

    void SpawnGoal()
    {
        GameObject goal = Instantiate(goalPrefab);

        goal.transform.position = new Vector3(
            transform.position.x,
            groundY + 3.0f,   // un poco por encima del suelo
            0f
        );
    }
}