using UnityEngine;

public class MageSpawner : MonoBehaviour
{
    public GameObject magePrefab;

    [Header("Timing")]
    public float spawnInterval = 3f;

    [Header("Spawn Y Range (arriba/medio)")]
    public float minY = 0.5f;
    public float maxY = 3.5f;

    private float timer;

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMage();
        }
    }

    private void SpawnMage()
    {
        if (magePrefab == null) return;

        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(transform.position.x, y, 0f);

        Instantiate(magePrefab, pos, Quaternion.identity);
    }
}