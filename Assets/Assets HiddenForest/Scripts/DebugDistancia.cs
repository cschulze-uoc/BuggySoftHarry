using UnityEngine;

public class DebugDistance : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            Debug.Log("Distancia al jugador: " + distance);
        }
    }
}
