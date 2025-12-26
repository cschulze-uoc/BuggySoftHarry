using UnityEngine;

public class ColorTren : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public Color colorTren;

    public void AsignarColor(Color col)
    {
        colorTren = col;
        foreach (var sprite in sprites)
        {
            if(sprite != null)
            {
                sprite.color = col;
            }
        }
    }
}
