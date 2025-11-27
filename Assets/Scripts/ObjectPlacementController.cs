using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.XR.Management; 

public class ObjectPlacementController : MonoBehaviour
{
    // Asegúrate de que este prefab esté conectado en el Inspector
    public GameObject objectToPlacePrefab; 
    
    private ARRaycastManager arRaycastManager;
    private GameObject placedObject;
    
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        // Iniciar la corrutina en Awake para que se ejecute antes que Start()
        StartCoroutine(StartARCore());

        // Usar el método actualizado para evitar la advertencia CS0618
        arRaycastManager = FindFirstObjectByType<ARRaycastManager>(); 
        
        if (arRaycastManager == null)
        {
            Debug.LogError("ARRaycastManager no encontrado. ¿Está el AR Session Origin en la escena?");
        }
    }

    System.Collections.IEnumerator StartARCore()
    {
        Debug.Log("Activando ARCore XR Loader...");
        var manager = XRGeneralSettings.Instance.Manager;

        // --- LÓGICA DE DESACTIVACIÓN DE CARDBOARD ---

        // 1. Verificar si ya hay un loader activo (que podría ser Cardboard)
        if (manager.activeLoader != null)
        {
            // Usamos la comparación de nombres, que siempre es accesible.
            if (manager.activeLoader.name.Contains("Cardboard"))
            {
                Debug.LogWarning("Cardboard Loader detectado como activo. Deteniendo subsistemas Cardboard...");

                // Primero detenemos los subsistemas del loader no deseado.
                manager.StopSubsystems();

                // Luego lo desinicializamos.
                manager.DeinitializeLoader();

                Debug.Log("Cardboard Loader detenido y desinicializado con éxito.");
            }
        }
        
        Debug.Log("Activando ARCore XR Loader...");
        // Inicializa el loader de AR
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            Debug.Log("ARCore Loader inicializado. Iniciando subsistemas...");
            // Inicia los subsistemas (cámara, rastreo, etc.)
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("ARCore XR iniciado y listo para usar.");
        }
        else
        {
            Debug.LogError("Error: ARCore Loader no se pudo inicializar. Verifique XR Plug-in Management.");
        }
    }
    
    void Update()
    {
        // Lógica de colocación (Solo si el Raycast Manager está activo y hay un toque)
        if (arRaycastManager != null && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            
            // Lanza un rayo de AR
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                // El rayo golpeó un plano detectado
                Pose hitPose = hits[0].pose;
                
                if (placedObject == null)
                {
                    // Primera colocación: instancia el objeto
                    placedObject = Instantiate(objectToPlacePrefab, hitPose.position, hitPose.rotation);
                    Debug.Log("Objeto colocado con éxito.");
                    
                    // Opcional: Deshabilita la detección de planos después de la primera colocación
                    // FindFirstObjectByType<ARPlaneManager>()?.enabled = false;
                }
                // Si ya está colocado, puedes moverlo o ignorar el toque.
            }
        }
    }
}