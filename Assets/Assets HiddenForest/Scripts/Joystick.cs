using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour,
    IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform handle;
    public float radius = 96f;

    private Vector2 input;

    public float Horizontal => input.x;
    public float Vertical => input.y;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        pos = Vector2.ClampMagnitude(pos, radius);
        handle.anchoredPosition = pos;
        input = pos / radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        input = Vector2.zero;
    }
}

