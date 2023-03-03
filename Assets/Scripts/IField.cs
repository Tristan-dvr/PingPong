using UnityEngine;

public interface IField
{
    float SafeWidth { get; }
    float Width { get; }
    Vector3 GetBallStartPosition();
    Vector3 GetPlayerRacketPosition();
    Vector3 GetEnemyRacketPosition();
    Vector3 GetForwardDirection();
}
