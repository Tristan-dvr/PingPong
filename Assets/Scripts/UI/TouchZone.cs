using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchZone : Graphic, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    private Vector2 _lastTouch;
    private bool _pressed;

    public Vector2 GetLastTouchScreenPosition() => _lastTouch;

    public bool IsPressed() => _pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        UpdateLastTouch(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        UpdateLastTouch(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
        UpdateLastTouch(eventData);
    }

    private void UpdateLastTouch(PointerEventData data)
    {
        _lastTouch = data.position;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }
}
