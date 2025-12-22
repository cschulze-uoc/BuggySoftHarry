using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private BoxCollider2D colRed;

    [Header("SonidoPasos")]
    [SerializeField] private AudioSource audioSource;

    [Header("SonidoRed")]
    [SerializeField] private AudioSource audioGolpe;
    [SerializeField] private AudioClip clipGolpe;

    private Rigidbody2D rig;
    private Vector2 movimiento;
    private Animator anim;
    private SpriteRenderer spritePersonaje;
    private float posColX = 1;
    private float posColY = 0;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spritePersonaje = GetComponentInChildren<SpriteRenderer>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
    }

    private void Update()
    {
        Movimiento();
        Captura();

        // Usar el parámetro "Anda" del Animator para activar/desactivar audio
        float velocidadAnim = anim.GetFloat("Anda");

        // Activar/desactivar audio de pasos
        if (velocidadAnim > 0.1f) // si el personaje se mueve
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else // personaje parado
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
    }

    private void FixedUpdate()
    {
        // Aplicar velocidad al Rigidbody2D
        rig.linearVelocity = movimiento;

        // Actualizar animacion
        Vector2 vel = rig.linearVelocity;

        anim.SetFloat("Anda", vel.magnitude);
        anim.SetFloat("MoveX", vel.x);
        anim.SetFloat("MoveY", vel.y);

        // Guardar última dirección, solo si se está moviendo
        if (vel.magnitude > 0.1f)
        {
            if (Mathf.Abs(vel.y) > Mathf.Abs(vel.x))
            {
                if (vel.y > 0)
                    anim.SetInteger("Direccion", 2); // Espaldas
                else
                    anim.SetInteger("Direccion", 0); // Frontal
            }
            else
            {
                anim.SetInteger("Direccion", 1); // Lateral
            }
        }

    }

    private void Movimiento()
    {
        // Leer input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcular vector de movimiento
        movimiento = new Vector2(horizontal, vertical) * velocidad;

        // Girar sprite segun direccion horizontal
        if (horizontal > 0)
        {
            colRed.offset = new Vector2(posColX, posColY);
            spritePersonaje.flipX = false;
        }
        else if (horizontal < 0)
        {
            colRed.offset = new Vector2(-posColX, posColY);
            spritePersonaje.flipX = true;
        }
    }

    private void Captura()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Captura");
        }
    }
}


