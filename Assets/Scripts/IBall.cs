using UnityEngine;

public interface IBall
{
    void SetVelocity(Vector3 direction, float speed);
    void SetPosition(Vector3 position);
    Vector3 GetPosition();
}
