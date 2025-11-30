using UnityEngine;

public class DementorController : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 1;
    int currentHealth;

    [Header("Movimiento")]
    public float moveSpeed = 0.3f;
    public float damageDistance = 0.7f;

    [Header("FX")]
    public GameObject deathVfxPrefab;   

    Transform target;          // cámara AR
    ARDementorGame gameManager;

    // Pequeño offset para que no vayan todos al mismo punto exacto
    Vector3 randomOffset;

    // Lo llamamos desde ARDementorGame.SpawnDementor()
    public void Init(ARDementorGame gm, Transform targetCamera, int health, float speed)
    {
        gameManager = gm;
        target = targetCamera;
        maxHealth = health;
        currentHealth = maxHealth;
        moveSpeed = speed;

        randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.3f, 0.3f),
            0f
        );
    }

    void Update()
    {
        if (target == null || gameManager == null || gameManager.IsGameOver())
            return;

        // Movimiento flotante hacia el jugador
        Vector3 targetPos = target.position + randomOffset;
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= damageDistance)
        {
            HitPlayer();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void HitPlayer()
    {
        gameManager?.PlayerHit(this);
        Destroy(gameObject);
    }

    void Die()
    {
        if (deathVfxPrefab != null)
        {
            Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
        }
        gameManager?.OnDementorKilled(this);
        Destroy(gameObject);
    }
}
