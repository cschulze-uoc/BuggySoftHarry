using UnityEngine;

public class AnimatorDebugger : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator != null)
        {
            // Esto hace que Unity "seleccione" el Animator automáticamente en el inspector
            // Solo funciona en modo Editor
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(animator);
#endif
        }
    }
}
