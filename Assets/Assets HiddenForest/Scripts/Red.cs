using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class PersonajeCaptura: MonoBehaviour
{
    private BoxCollider2D colRed;

    private void Awake()
    {
        colRed = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("AnimalHerido"))
        {
            Destroy(otro.gameObject);
        }
    }

}
