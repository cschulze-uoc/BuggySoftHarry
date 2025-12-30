using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float tiempoInicial = 120f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoTiempo;

    // NUEVO: referencia al Contador (arrástralo en el inspector)
    [SerializeField] private Contador contador;

    private float tiempoRestante;
    private bool juegoActivo = true;

    // NUEVO
    private int baseGlobalScore = 0;

    private void Start()
    {
        tiempoRestante = tiempoInicial;

        // NUEVO: leer global al entrar
        baseGlobalScore = (GlobalGameManager.Instance != null) ? GlobalGameManager.Instance.totalScore : 0;

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

        Time.timeScale = 0f;
        Debug.Log("FIN DEL JUEGO");

        // NUEVO: obtener puntuación local desde Contador
        int puntosLocal = (contador != null) ? contador.puntos : 0;

        int finalScore = baseGlobalScore + puntosLocal;

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.totalScore = finalScore;

            // IMPORTANTE: reactivar el timeScale antes de cambiar de escena
            Time.timeScale = 1f;
            GlobalGameManager.Instance.GoToNextMinigame();
        }
        else
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("00_MainMenu");
        }
    }
}
