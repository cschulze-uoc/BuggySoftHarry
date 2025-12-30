using UnityEngine;
using UnityEngine.InputSystem; // NEW INPUT SYSTEM

public class BroomController : MonoBehaviour
{
    public float jumpForce = 6f;
    private Rigidbody2D rb;
    public bool isAlive = true;

    [Header("Audio")]
    public AudioSource deathAudio;   // 💥 sonido de muerte

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive) return;

        // --- Teclado: Space
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
            return;
        }

        // --- Ratón: click izquierdo
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Jump();
            return;
        }

        // --- Móvil: tap
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                Jump();
                return;
            }
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        if (!isAlive) return;

        isAlive = false;

        if (deathAudio != null)
            deathAudio.Play();

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (QDGameManager.Instance != null)
            QDGameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
            return;

        Die();
    }
}
