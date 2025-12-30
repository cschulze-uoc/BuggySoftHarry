using UnityEngine;

public class Switch : MonoBehaviour
{
    public Tramo tramo1;
    public Tramo tramo2;
    public bool haciaTramo1 = false;

    public GameObject viaTramo1;
    public GameObject viaTramo2;

    private void Start()
    {
        ActualizarVias();
    }

    public Tramo SiguienteTramo()
    {
        return haciaTramo1 ? tramo1 : tramo2;
    }

    public void Toggle()
    {
        haciaTramo1 = !haciaTramo1;
        ActualizarVias();
    }

    private void ActualizarVias()
    {
        viaTramo1.SetActive(haciaTramo1);
        viaTramo2.SetActive(!haciaTramo1);
    }
}