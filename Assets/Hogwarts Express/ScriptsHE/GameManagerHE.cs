using UnityEngine;
using TMPro;
using System.Collections;

public class GameManagerHE : MonoBehaviour
{
    public static GameManagerHE instance;
    public int trenesBien = 0;
    public int puntos = 0;
    public TMP_Text textoPuntuacion;
    public GameObject panelGO;
    public TMP_Text puntuacionFinal;

    private int trenesActivos = 0;
    private bool audioStarted = false;

    // NUEVO: score global al entrar
    private int baseGlobalScore = 0;

    // NUEVO: evitar doble GameOver
    private bool finished = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // NUEVO: leer puntuaci�n global al entrar
        if (GlobalGameManager.Instance != null)
            baseGlobalScore = GlobalGameManager.Instance.totalScore;
        else
            baseGlobalScore = 0;

        ActualizarTexto();
    }

    public void TrenCorrecto()
    {
        trenesBien++;
        puntos = puntos + 10;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        // NUEVO: mostrar global + local
        int displayScore = baseGlobalScore + puntos;
        textoPuntuacion.text = "Trenes correctos: " + trenesBien + "\nPuntos: " + displayScore;
    }

    public void GameOver()
    {
        if (finished) return;
        finished = true;

        Time.timeScale = 0f;
        StartCoroutine(MostrarGO());
        AudioManagerHE.Instance.StopLocomotora();
        AudioManagerHE.Instance.StopBocina();
        AudioManagerHE.Instance.GameOverSound();
    }

    IEnumerator MostrarGO()
    {
        yield return new WaitForSecondsRealtime(1f);
        panelGO.SetActive(true);

        // NUEVO: puntuaci�n final global + local
        int finalScore = baseGlobalScore + puntos;
        puntuacionFinal.text = "Trenes correctos: " + trenesBien + "\nPuntos totales: " + finalScore;

        // NUEVO: esperar un momento y pasar al siguiente juego
        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1f;

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.totalScore = finalScore;
            Debug.Log($"Puntuación actualizada a {GlobalGameManager.Instance.totalScore}");
            GlobalGameManager.Instance.GoToNextMinigame();
        }
        else
        {
            // si se ejecuta suelto fuera de campa�a
            UnityEngine.SceneManagement.SceneManager.LoadScene("00_MainMenu");
        }
    }

    public void ResgistrarTren()
    {
        trenesActivos++;
        if (!audioStarted && trenesActivos == 1)
        {
            audioStarted = true;
            StartCoroutine(StartAudioAfterDelay(0.3f));
        }
    }

    private IEnumerator StartAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioManagerHE.Instance.StartLocomotora();
        AudioManagerHE.Instance.StartBocina();
    }
}
