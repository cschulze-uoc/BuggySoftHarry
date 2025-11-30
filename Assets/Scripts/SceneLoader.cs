using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Función pública para que los botones puedan llamarla.
    public void LoadSceneByName(string sceneName)
    {
        // Usa SceneManager para cargar la escena cuyo nombre le pasamos.
        SceneManager.LoadScene(sceneName);
    }
    
    // Función de ejemplo para salir del juego (para el botón 'Exit' que podrías añadir)
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}