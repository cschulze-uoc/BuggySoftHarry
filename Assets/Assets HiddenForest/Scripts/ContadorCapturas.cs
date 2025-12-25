using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    public int puntos = 0;
    public TMP_Text contadorTexto;

    public void CapturarAnimal(Animal animal)
    {
        if (animal.estaHerido)
        {
            puntos += 1;
        }
        else
        {
            puntos -= 1;
        }
        contadorTexto.text = "Capturados: " + puntos;
    }
}

