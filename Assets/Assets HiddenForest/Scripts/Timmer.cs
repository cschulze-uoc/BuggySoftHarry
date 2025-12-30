using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoInicial = 120f; // 2 minutos

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoTiempo;

    private float tiempoRestante;
    private bool juegoActivo = true;

    private void Start()
    {
        tiempoRestante = tiempoInicial;
        ActualizarTexto();
    }

    private void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            FinDelJuego();
        }

        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

        textoTiempo.text = $"{minutos:00}:{segundos:00}";
    }

    private void FinDelJuego()
    {
        juegoActivo = false;

        // Detener el tiempo del juego
        Time.timeScale = 0f;

        Debug.Log("FIN DEL JUEGO");

        // Por añadir:
        // - Mostrar puntuación
        // - Cambiar a juego siguiente
    }
}
