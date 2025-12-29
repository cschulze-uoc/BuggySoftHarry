using UnityEngine;
using System.Collections;

public class TrainSpawner : MonoBehaviour
{
    public GameObject trenPrefab;
    public Transform spawnPoint;
    private float spawnInterval = 3;
    private float timer = 0;
    private float tiempoCambio = 10f;
    private float reduccion = 0.2f;
    private float timerDificultad = 0f;

    public Transform puntoA;
    public Transform puntoB;

    public Color[] coloresDisponibles;
    
    void Start()
    {
        spawnTrain();
    }

    void Update()
    {
        if(timer < spawnInterval)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnTrain();
        }

        timerDificultad += Time.deltaTime;
        if(timerDificultad > tiempoCambio)
        {
            spawnInterval -= reduccion;
            timerDificultad = 0f;
        }
    }

    public void spawnTrain()
    {
        GameObject nuevoTren = Instantiate(trenPrefab, spawnPoint.position, Quaternion.identity);
        Color colorElegido = coloresDisponibles[Random.Range(0, coloresDisponibles.Length)];
        ColorTren colorTren = nuevoTren.GetComponent<ColorTren>();
        colorTren.AsignarColor(colorElegido);
        
        TrainMover trenScript = nuevoTren.GetComponent<TrainMover>();
        trenScript.puntos.Add(puntoA);
        trenScript.puntos.Add(puntoB);

        timer = 0;

        GameManagerHE.instance.ResgistrarTren();
    }
}
