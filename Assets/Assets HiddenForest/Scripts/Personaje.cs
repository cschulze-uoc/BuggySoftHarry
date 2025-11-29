using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private BoxCollider2D colRed;

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
    }

    private void Update()
    {
        Movimiento();
        Captura();
    }

    private void FixedUpdate()
    {
        // Aplicar velocidad al Rigidbody2D
        rig.linearVelocity = movimiento;

        // Actualizar animacion
        anim.SetFloat("Anda", Mathf.Abs(rig.linearVelocity.magnitude));
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


