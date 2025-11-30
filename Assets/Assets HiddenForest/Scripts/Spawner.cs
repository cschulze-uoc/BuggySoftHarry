using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalesPrefabs;
    public Transform[] posiciones;

    public float intervalo = 5f;
    public float tiempoVida = 10f;


    private List<GameObject> animalesEnPantalla = new List<GameObject>();
    private HashSet<int> posicionesOcupadas = new HashSet<int>();

    void Start()
    {
        StartCoroutine(SpawnAnimals());
    }

    IEnumerator SpawnAnimals()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            // animales disponibles
            List<GameObject> animalesDisponibles = new List<GameObject>();
            foreach (var animalPrefab in animalesPrefabs)
            {
                bool yaExiste = animalesEnPantalla.Exists(a =>
                    a != null && a.name.Contains(animalPrefab.name));

                if (!yaExiste)
                    animalesDisponibles.Add(animalPrefab);
            }

            if (animalesDisponibles.Count == 0)
                continue;

            // posiciones libres
            List<int> posicionesLibres = new List<int>();
            for (int i = 0; i < posiciones.Length; i++)
            {
                if (!posicionesOcupadas.Contains(i))
                    posicionesLibres.Add(i);
            }

            if (posicionesLibres.Count == 0)
                continue;

            // elegir animal
            GameObject animalPrefabElegido =
                animalesDisponibles[Random.Range(0, animalesDisponibles.Count)];

            // elegir posicion visible
            int indexPos = posicionesLibres[Random.Range(0, posicionesLibres.Count)];
            Transform posVisible = posiciones[indexPos];

            posicionesOcupadas.Add(indexPos);


            // Instanciar en la posicion oculta
            GameObject animal = Instantiate(animalPrefabElegido, posVisible.position, Quaternion.identity);
            animalesEnPantalla.Add(animal);

            // Iniciar secuencia completa
            StartCoroutine(DesaparecerAnimal(animal, indexPos));
        }
    }

    IEnumerator DesaparecerAnimal(GameObject animal, int indexPos)
    {
        yield return new WaitForSeconds(tiempoVida);

        Destroy(animal);
        posicionesOcupadas.Remove(indexPos);
        animalesEnPantalla.Remove(animal);
    }
}
