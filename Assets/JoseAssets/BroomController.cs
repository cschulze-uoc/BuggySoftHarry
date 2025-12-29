using UnityEngine;
using UnityEngine.InputSystem;   //  NEW INPUT SYSTEM

public class BroomController : MonoBehaviour
{
    public float jumpForce = 6f;
    private Rigidbody2D rb;
    public bool isAlive = true;

    [Header("Audio")]
    public AudioSource deathAudio;   //  sonido de muerte

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive) return;

        // ---------- TECLADO (PC) ----------
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        // ---------- RATÓN ----------
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Jump();
        }

        // ---------- TÁCTIL (MÓVIL) ----------
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        // Reset vertical para salto consistente
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        if (!isAlive) return;

        isAlive = false;

        // 💥 SONIDO DE MUERTE
        if (deathAudio != null)
            deathAudio.Play();

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        QDGameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
            return;

        Die();
    }
}