using UnityEngine;
using System.Collections.Generic;

public class TrainMover : MonoBehaviour
{
    public List<Transform> puntos = new List<Transform>();
    public float speed = 2f;
    private int currentIndex = 0;

    public GameObject spriteU;
    public GameObject spriteD;
    public GameObject spriteL;
    public GameObject spriteR;
    public GameObject humoU;
    public GameObject humoD;
    public GameObject humoL;
    public GameObject humoR;
    void Start()
    {
        if (puntos == null || puntos.Count == 0)
        {
            Debug.LogWarning("TrainMover: no hay puntos asignados.");
            return;
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

        Vector2 direccion = (target.position - transform.position).normalized;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            if (direccion.x > 0)
            {
                ActiviarSpriteTren(spriteR);
            }
            else
            {
                ActiviarSpriteTren(spriteL);
            }
        }
        else
        {
            if (direccion.y > 0)
            {
                ActiviarSpriteTren(spriteU);
            }
            else
            {
                ActiviarSpriteTren(spriteD);
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= puntos.Count)
            {
                finalRuta(); 
            }
        }
    }

    void finalRuta()
    {
        currentIndex = puntos.Count - 1;
        Destroy(gameObject, 0.1f);
    }

    public void seleccionarRuta(List<Transform> nuevaRuta)
    {
        puntos = nuevaRuta;
        currentIndex = 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //bifurcación
        Switch bif = other.GetComponent<Switch>();
        if(bif != null)
        {
            seleccionarRuta(bif.SiguienteTramo().puntos);
            return;
        }
        //curva
        Curva curva = other.GetComponent<Curva>();
        if(curva != null)
        {
            seleccionarRuta(curva.SiguienteTramo().puntos);
            return;
        }
        //Estacion
        Estacion estacion = other.GetComponent<Estacion>();
        if(estacion != null)
        {
            ColorTren colTren = GetComponent<ColorTren>();
            ComprobarEstacion(estacion, colTren);
        }
    }

    private void ActiviarSpriteTren(GameObject activo)
    {
        spriteU.SetActive(activo == spriteU);
        humoU.SetActive(activo == spriteU);
        spriteD.SetActive(activo == spriteD);
        humoD.SetActive(activo == spriteD);
        spriteL.SetActive(activo == spriteL);
        humoL.SetActive(activo == spriteL);
        spriteR.SetActive(activo == spriteR);
        humoR.SetActive(activo == spriteR);
    }

    void ComprobarEstacion(Estacion estacion, ColorTren colTren)
    {
        if (coloresCoinciden(estacion.colorEstacion, colTren.colorTren))
        {
            GameManagerHE.instance.TrenCorrecto();
            AudioManagerHE.Instance.LlegadaCorrecta();
        }
        else
        {
            GameManagerHE.instance.GameOver();
        }
    }

    bool coloresCoinciden(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
            Mathf.Approximately(a.g, b.g) &&
            Mathf.Approximately(a.b, b.b);
    }
}
