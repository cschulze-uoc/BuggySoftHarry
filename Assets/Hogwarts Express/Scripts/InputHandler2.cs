using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler2 : MonoBehaviour
{
    public static InputHandler2 Instance { get; private set; }

    private void Awake() => Instance = this;

    public bool TryGetPointerDown(out Vector2 worldPos)
    {
        worldPos = Vector2.zero;

        // Touch
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            return true;
        }

        // Mouse
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            return true;
        }

        return false;
    }
}
