using System.Collections;
using UnityEngine;

public class HorrocruxAppearDisappear : MonoBehaviour
{
    private Material mat;
    private Color baseColor;
    private Vector3 initialScale;

    public float duration = 0.6f;

    private void Awake()
    {
        mat = GetComponentInChildren<Renderer>().material;
        baseColor = mat.color;
        initialScale = transform.localScale;
    }

    public void PlayDisappear()
    {
        StopAllCoroutines();
        StartCoroutine(DisappearRoutine());
    }

    public void PlayAppear()
    {
        StopAllCoroutines();
        StartCoroutine(AppearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        float t = 0;
        Vector3 startScale = initialScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;

            // Transparencia
            Color c = baseColor;
            c.a = Mathf.Lerp(1f, 0f, k);
            mat.color = c;

            // Reducir tamaño
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, k);

            yield return null;
        }

        // Al final, NO desactivar: solo dejarlo invisible
        transform.localScale = Vector3.zero;
    }

    private IEnumerator AppearRoutine()
    {
        // Estado inicial invisible
        transform.localScale = Vector3.zero;

        Color c = baseColor;
        c.a = 0;
        mat.color = c;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;

            // Opacidad
            Color col = baseColor;
            col.a = Mathf.Lerp(0f, 1f, k);
            mat.color = col;

            // Tamaño
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, k);

            yield return null;
        }

        // Asegurar estado final correcto
        transform.localScale = initialScale;
        mat.color = baseColor;
    }
}
