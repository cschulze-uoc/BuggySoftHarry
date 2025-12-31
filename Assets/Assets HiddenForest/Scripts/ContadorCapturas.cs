using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    public int puntos = 0;
    public TMP_Text contadorTexto;

    private void Start()
    {
        if (GlobalGameManager.Instance != null)
            puntos = GlobalGameManager.Instance.totalScore;
        else
            puntos = 0;
    }
    public void CapturarAnimal(Animal animal)
    {
        if (animal.estaHerido)
        {
            puntos += 10;
        }
        else
        {
            puntos -= 5;
        }
        contadorTexto.text = "Puntos: " + puntos;
    }

    public void Inicializar()
    {
        contadorTexto.text = "Puntos: " + puntos;
        
    }
}

