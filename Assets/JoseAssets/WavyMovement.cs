using UnityEngine;

public class WavyMovement : MonoBehaviour
{
    [Header("Horizontal")]
    public float speed = 3f;
    public float destroyX = -10f;

    [Header("Wave")]
    public float amplitude = 0.6f;   // cuánto sube/baja
    public float frequency = 2f;     // qué rápido sube/baja

    private float baseY;
    private float seed;

    private void Start()
    {
        baseY = transform.position.y;
        seed = Random.Range(0f, 100f); // para que no todos hagan la misma onda
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        // mover izquierda
        transform.position += Vector3.left * speed * Time.deltaTime;

        // onda vertical
        float y = baseY + Mathf.Sin((Time.time + seed) * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        // destruir al salir
        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}