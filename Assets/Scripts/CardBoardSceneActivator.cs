using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections; // Necesario para IEnumerator y Yield

public class CardboardSceneActivator : MonoBehaviour
{
    void Start()
    {
        // Iniciamos la rutina de inicialización.
        // Hacemos que se retrase un frame con yield return null.
        StartCoroutine(ActivateCardboard());
    }

    IEnumerator ActivateCardboard()
    {
        Debug.Log("Iniciando Cardboard XR...");

        // 1. ESPERA DE UN FRAME: Permite que el ciclo de vida de la escena de Unity se complete.
        yield return null; 

        // 2. Inicializar el XR Loader (Cardboard Loader)
        // Esta función es ASÍNCRONA y puede tardar varios frames.
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        // 3. Verificar la inicialización
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Error: Cardboard Loader no se pudo inicializar.");
            // Opcional: Cargar una escena de error o volver al menú.
            yield break; // Detener la corrutina si falla
        }

        Debug.Log("Cardboard Loader inicializado. Iniciando subsistemas...");
        
        // 4. Iniciar los subsistemas XR (Giroscopio/Cámara/Rastreo)
        XRGeneralSettings.Instance.Manager.StartSubsystems();

        Debug.Log("Cardboard XR iniciado y listo para usar.");

        // Opcional: Activar la cámara VR o el Gaze Input si es necesario.
        // Si tienes el prefab CardboardMain, ya debería activarse.
    }

    void OnDestroy()
    {
        // Detener los subsistemas XR cuando la escena se destruye (volviendo al menú).
        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            Debug.Log("Cardboard XR detenido.");
        }
    }
}