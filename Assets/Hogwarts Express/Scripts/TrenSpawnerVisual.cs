using System.Collections;
using UnityEngine;

public class TrenSpawnerVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] renderers;
    [SerializeField] private float delay = 0.5f;

    private void Awake()
    {
        HacerVisible(false);
        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        HacerVisible(true);
    }

    private void HacerVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}
