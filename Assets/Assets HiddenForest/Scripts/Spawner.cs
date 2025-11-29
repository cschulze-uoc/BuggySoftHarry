using UnityEngine;
using System.Collections;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalesPrefabs;  // prefabs de los animales
    public Transform[] posiciones;         // posiciones posibles
    public float intervalo = 5f;           // cada cuanto aparece un animal
    public float tiempoVida = 10f;         // tiempo antes de que el animal desaparezca si no lo capturan

    void Start()
    {
        StartCoroutine(SpawnAnimals());
    }

    IEnumerator SpawnAnimals()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            // Elegir animal aleatorio
            int indiceAnimal = Random.Range(0, animalesPrefabs.Length);
            GameObject prefab = animalesPrefabs[indiceAnimal];

            // Elegir posicion aleatoria
            int indicePos = Random.Range(0, posiciones.Length);
            Vector3 spawnPos = posiciones[indicePos].position;

            // Instanciar animal
            GameObject animal = Instantiate(prefab, spawnPos, Quaternion.identity);

            // Iniciar coroutine para que desaparezca si no es capturado
            StartCoroutine(DesaparecerAnimal(animal));
        }
    }

    IEnumerator DesaparecerAnimal(GameObject animal)
    {
        yield return new WaitForSeconds(tiempoVida);

        if (animal != null)
        {
            // Ejemplo: mover un poco antes de destruir
            animal.transform.Translate(Vector3.forward * 2f);
            Destroy(animal, 0.5f);
        }
    }
}
