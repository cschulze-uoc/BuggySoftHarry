using UnityEngine;
using System.Collections.Generic;

public class TrainMover : MonoBehaviour
{
    public List<Transform> puntos;
    //public Transform[] pathPoints;
    public float speed = 2f;
    private int currentIndex = 0;
    void Start()
    {
        if (puntos == null || puntos.Count == 0)
        {
            Debug.LogWarning("TrainMover: no hay puntos asignados.");
        }
        else
        {
            transform.position = puntos[0].position;
        }
    }

    void Update()
    {
        if (puntos == null || puntos.Count == 0)
        {
            return;
        }
        Transform target = puntos[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= puntos.Count)
            {
                finalRuta();
                //Destroy(gameObject, 0.1f);
            }
        }
    }

    void finalRuta()
    {
        currentIndex = puntos.Count - 1;
    }

    public void seleccionarRuta(List<Transform> nuevaRuta)
    {
        puntos = nuevaRuta;
        currentIndex = 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Switch bif = other.GetComponent<Switch>();
        if(bif != null)
        {
            seleccionarRuta(bif.SiguienteTramo().puntos);
        }
    }
}
