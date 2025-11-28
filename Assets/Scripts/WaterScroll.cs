using UnityEngine;

public class WaterScroll : MonoBehaviour
{
    public float scrollSpeed = 0.02f;
    private Renderer rend;
    private Vector2 offset = Vector2.zero;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        offset.y += scrollSpeed * 0.5f * Time.deltaTime;
        rend.material.SetTextureOffset("_BaseMap", offset);
        rend.material.SetTextureOffset("_BumpMap", offset);
    }
}
