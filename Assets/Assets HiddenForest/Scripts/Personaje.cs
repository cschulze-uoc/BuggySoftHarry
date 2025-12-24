using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class Personaje : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private BoxCollider2D colRed;

    private Rigidbody2D rig;
    private Vector2 movimiento;
    private Animator anim;
    private SpriteRenderer spritePersonaje;

    [Header("Joystick")]
    [SerializeField] private Joystick joystick;

    Vector2 inputFinal;

    private Vector2 inputTeclado;   // Teclado
    private Vector2 inputJoystick;  // Joystick en pantalla


    private float posColX = 1;
    private float posColY = 0;

    [Header("SonidoPasos")]
    [SerializeField] private AudioSource audioSource;

    [Header("SonidoRed")]
    [SerializeField] private AudioSource audioGolpe;
    [SerializeField] private AudioClip clipGolpe;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spritePersonaje = GetComponentInChildren<SpriteRenderer>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
    }

    private void FixedUpdate()
    {
        // Leer joystick virtual
        if (joystick != null)
        {
            inputJoystick = new Vector2(
                joystick.Horizontal,
                joystick.Vertical
            );
        }

        Vector2 inputFinal = inputJoystick.magnitude > 0.1f ? inputJoystick : inputTeclado;
        Movimiento(inputFinal);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputTeclado = context.ReadValue<Vector2>();
    }

    public void OnCapture(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            anim.SetTrigger("Captura");

            // Reproducir sonido de golpe si está configurado
            if (audioGolpe != null && clipGolpe != null)
            {
                audioGolpe.PlayOneShot(clipGolpe);
            }
        }
    }

    private void Movimiento(Vector2 input)
    {
        // Movimiento final
        movimiento = input * velocidad;

        // Aplicar velocidad
        rig.linearVelocity = movimiento;

        // Girar sprite
        if (input.x > 0)
        {
            spritePersonaje.flipX = false;
            colRed.offset = new Vector2(posColX, posColY);
        }
        else if (input.x < 0)
        {
            spritePersonaje.flipX = true;
            colRed.offset = new Vector2(-posColX, posColY);
        }

        // Animaciones
        Vector2 vel = rig.linearVelocity;
        anim.SetFloat("Anda", vel.magnitude);
        anim.SetFloat("MoveX", vel.x);
        anim.SetFloat("MoveY", vel.y);

        if (vel.magnitude > 0.1f)
        {
            if (Mathf.Abs(vel.y) > Mathf.Abs(vel.x))
                anim.SetInteger("Direccion", vel.y > 0 ? 2 : 0);
            else
                anim.SetInteger("Direccion", 1);
        }

        // Audio pasos
        float velocidadAnim = anim.GetFloat("Anda");
        if (velocidadAnim > 0.1f)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
    }

}


