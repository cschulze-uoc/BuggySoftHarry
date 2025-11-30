using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int animalesCapturados = 0;
    public TMP_Text contadorTexto;

    public void SumarAnimal()
    {
        animalesCapturados++;
        contadorTexto.text = "Capturados: " + animalesCapturados;
    }
}

