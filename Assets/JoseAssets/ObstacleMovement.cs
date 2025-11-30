using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 3f;
    public float destroyX = -10f;

    void Update()
    {
        if (Time.timeScale == 0f) return; //  DETENER EN GAMEOVER / VICTORIA

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}