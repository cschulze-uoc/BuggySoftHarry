using UnityEngine;

public class PersonajeCaptura: MonoBehaviour
{
    private Contador contador;

    private void Start()
    {
        contador = FindObjectOfType<Contador>();
    }


    private void OnTriggerEnter2D(Collider2D otro)
    {
        Animal animal = otro.GetComponent<Animal>();

        if (animal != null)
        {
            contador.CapturarAnimal(animal);
            Destroy(otro.gameObject);
        }
    }

}
