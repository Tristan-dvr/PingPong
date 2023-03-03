using UnityEngine;
using Zenject;

public class Field : MonoBehaviour, IField, IInitializable
{
    public float width = 5;
    public Vector2 referenceScreenResolution = new Vector2(9, 16);
    [Space]
    public Transform playerRacket;
    public Transform enemyRacket;
    public Transform ballStart;
    [Space]
    public Gates playerGates;
    public Gates enemyGates;
    [Space]
    public float boarderSize = 1;
    public Transform leftBoarder;
    public Transform rightBoarder;

    public float SafeWidth { get; private set; }
    public float Width { get; private set; }

    private Vector3 _forwardDirection;

    public void Initialize()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        var aspectRatio = Screen.width / (float)Screen.height;
        var referenceRatio = referenceScreenResolution.x / referenceScreenResolution.y;
        var offset = aspectRatio / referenceRatio * width;
        leftBoarder.localPosition = Vector3.left * offset;
        rightBoarder.localPosition = Vector3.right * offset;
        Width = width;
        SafeWidth = offset * 2 - boarderSize;

        _forwardDirection = (GetEnemyRacketPosition() - GetBallStartPosition()).normalized;
    }

    public Vector3 GetBallStartPosition() => ballStart.position;

    public Vector3 GetPlayerRacketPosition() => playerRacket.position;

    public Vector3 GetEnemyRacketPosition() => enemyRacket.position;

    public Vector3 GetForwardDirection() => _forwardDirection;
}
