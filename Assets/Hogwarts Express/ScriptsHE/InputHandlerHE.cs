using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private InputActionReference pointAction;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        pointAction.action.Enable();
    }

    private void OnDisable()
    {
        pointAction.action.Disable();
    }

    void Update()
    {
        if (InputHandler2.Instance.TryGetPointerDown(out Vector2 worldPos))
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPos);
            if (!hit) return;

            Switch sw = hit.GetComponent<Switch>();
            if (sw) sw.Toggle();
        }
    }
}
