using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Curva : MonoBehaviour
{
    public Tramo tramo;

    public Tramo SiguienteTramo()
    {
        return tramo;
    }
}