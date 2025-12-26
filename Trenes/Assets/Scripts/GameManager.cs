using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int trenesBien = 0;
    public int puntos = 0;
    public TMP_Text textoPuntuacion;
    public GameObject panelGO;
    public TMP_Text puntuacionFinal;

    private int trenesActivos = 0;
    private bool audioStarted = false;
    private void Awake()
    {
        if(instance == null)
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
        textoPuntuacion.text = "Trenes correctos: " + trenesBien + "\nPuntos: " + puntos;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        StartCoroutine(MostrarGO());
        AudioManager.Instance.StopLocomotora();
        AudioManager.Instance.StopBocina();
        AudioManager.Instance.GameOverSound();
    }

    IEnumerator MostrarGO()
    {
        yield return new WaitForSecondsRealtime(1f);
        panelGO.SetActive(true);
        puntuacionFinal.text = "Trenes correctos: " + trenesBien + "\nPuntos totales: " + puntos;
    }

    public void ResgistrarTren()
    {
        trenesActivos++;
        if(!audioStarted && trenesActivos == 1)
        {
            audioStarted = true;
            StartCoroutine(StartAudioAfterDelay(0.3f));
        }
    }

    private IEnumerator StartAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioManager.Instance.StartLocomotora();
        AudioManager.Instance.StartBocina();
    }
}
