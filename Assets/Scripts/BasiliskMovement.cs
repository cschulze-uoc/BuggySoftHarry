using UnityEngine;

public class BasiliskMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 12f;
    public float arriveDistance = 0.5f;
    public float waitAtPoint = 0.1f;

    private int currentIndex = 0;
    private float waitTimer = 0f;
    private int direction = 1; // 1 = hacia adelante, -1 = hacia atrás

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("El basilisco no tiene waypoints asignados.");
            enabled = false;
            return;
        }

        // Empieza en el primer punto
        currentIndex = 0;
        transform.position = waypoints[currentIndex].position;
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];

        // Movimiento
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // Comprobación de llegada
        float dist = Vector3.Distance(transform.position, target.position);

        if (dist < arriveDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitAtPoint)
            {
                waitTimer = 0f;

                if (waypoints.Length > 1)
                {
                    // Si estamos en el primero, vamos hacia adelante
                    if (currentIndex == 0)
                        direction = 1;
                    // Si estamos en el último, vamos hacia atrás
                    else if (currentIndex == waypoints.Length - 1)
                        direction = -1;

                    currentIndex += direction;
                }
            }
        }
    }
}
