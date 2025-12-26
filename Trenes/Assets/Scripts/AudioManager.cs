using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [Header("Clips")]
    [SerializeField] private AudioClip bocina;

    [Header("Settings")]
    [SerializeField] private float minInterBocina = 12f;
    [SerializeField] private float maxInterBocina = 18f;

    [SerializeField] private AudioSource locomotoraSource;
    [SerializeField] private AudioSource bocinaSource;
    [SerializeField] private AudioSource campanaSource;
    [SerializeField] private AudioSource GOSource;

    private Coroutine bocinaCorrutina;

    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartLocomotora()
    {
        if (!locomotoraSource.isPlaying)
        {
            locomotoraSource.Play();
        }
    }

    public void StopLocomotora()
    {
        if (locomotoraSource.isPlaying)
        {
            locomotoraSource.Stop();
        }
    }

    public void StartBocina()
    {
        if(bocinaCorrutina != null)
        {
            StopCoroutine(bocinaCorrutina);
        }
        bocinaCorrutina = StartCoroutine(BocinaRutina());
    }

    public void StopBocina()
    {
        if(bocinaCorrutina != null)
        {
            StopCoroutine(bocinaCorrutina);
            bocinaCorrutina = null;
        }
        bocinaSource.Stop();
    }

    private IEnumerator BocinaRutina()
    {
        while (true)
        {
            float wait = Random.Range(minInterBocina, maxInterBocina);
            yield return new WaitForSeconds(wait);

            bocinaSource.PlayOneShot(bocina);
        }
    }

    public void LlegadaCorrecta()
    {
        campanaSource.Play();
    }

    public void GameOverSound()
    {
        GOSource.Play();
    }
}
