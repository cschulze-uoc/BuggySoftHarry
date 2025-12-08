using UnityEngine;

public class BroomController : MonoBehaviour
{
    public float jumpForce = 6f;
    private Rigidbody2D rb;
    public bool isAlive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive) return;

        // --- PC (ratón y teclado)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // --- MÓVIL (pantalla táctil)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // reset vertical para que el salto sea consistente
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f; // evita caída infinita

        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
            return;

        Die();
    }
}