using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.5f;

    public float minHeight = 1.0f;
    public float maxHeight = 5.0f;

    public float groundY = -3.7f;

    private float timer;

    void Update()
    {
        if (Time.timeScale == 0f) return; // NO SPAWNEAR si el juego terminó

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
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
}