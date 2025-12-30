using System;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public bool estaHerido; // true = herido, false = sano

    private void OnTriggerEnter2D(Collider2D otro)
    {
        Animal animal = otro.GetComponent<Animal>();

        if (animal != null)
        {
            // contador.CapturarAnimal(animal);
            Debug.Log("Capturado animal!");
            Destroy(otro.gameObject);
        }
    }
}
