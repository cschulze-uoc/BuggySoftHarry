using UnityEngine;

public class Switch : MonoBehaviour
{
    public Tramo tramo1;
    public Tramo tramo2;
    public bool haciaTramo1 = false;

    public void OnMouseDown()
    {
        haciaTramo1 = !haciaTramo1;
        Debug.Log("Switch cambiado a: " + (haciaTramo1 ? "Tramo 1" : "Tramo 2"));
    }

    public Tramo SiguienteTramo()
    {
        return haciaTramo1 ? tramo1 : tramo2;
    }
}
