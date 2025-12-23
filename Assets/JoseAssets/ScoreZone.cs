using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private bool scored = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (scored) return;

        if (collision.CompareTag("Player"))
        {
            scored = true;
            QDGameManager.Instance.AddScore(5);
        }
    }
}