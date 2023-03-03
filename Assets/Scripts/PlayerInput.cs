using UnityEngine;

public class PlayerInput : IRacketInput
{
    private TouchZone _touchZone;

    public PlayerInput(TouchZone touchZone)
    {
        _touchZone = touchZone;
    }

    public float GetPosition()
    {
        if (IsPressed())
        {
            var touchPosition = _touchZone.GetLastTouchScreenPosition();
            return Mathf.Lerp(-1, 1, touchPosition.x / Screen.width);
        }
        else
        {
            return 0f;
        }
    }

    public bool IsPressed() => _touchZone.IsPressed();
}
