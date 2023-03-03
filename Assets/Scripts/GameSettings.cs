using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public float startBallSpeed = 4;
    public float maxStartAngle = 60;
    [Space]
    public float collisionSpeedBoost = 0.1f;
    public float maxBallReflectionAngle = 80;
}
