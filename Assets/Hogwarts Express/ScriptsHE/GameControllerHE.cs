using UnityEngine;

public class GameController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Switch sw = hit.collider.GetComponent<Switch>();
                if (sw != null)
                {
                    sw.Toggle();
                }
            }
        }
    }
}
