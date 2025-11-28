using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips")]
    public AudioClip hoverClip;
    public AudioClip collectClip;

    private AudioSource source;

    void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    // === SONIDO DE APUNTADO / HOVER ===
    public void PlayHover()
    {
        if (source.clip == hoverClip && source.isPlaying)
            return;

        source.loop = true;
        source.clip = hoverClip;
        source.Play();
    }

    public void StopHover()
    {
        if (source.clip == hoverClip)
            source.Stop();
    }

    // === SONIDO DE RECOLECCIÓN ===
    public void PlayCollect()
    {
        source.loop = false;
        source.PlayOneShot(collectClip);
    }
}
