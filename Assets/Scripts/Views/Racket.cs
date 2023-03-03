using UnityEngine;
using Zenject;

public class Racket : MonoBehaviour, IRacket
{
    public PlayerType type;
    public float size = 1;
    public float speed = 1;

    private IRacketInput _input;
    private IField _field;
    private MatchStateHandler _matchState;
    private GamePauseHandler _pauseHandler;

    [Inject]
    protected void Construct(DiContainer container, IField field, MatchStateHandler matchState, GamePauseHandler pauseHandler)
    {
        _input = container.TryResolveId<IRacketInput>(type);
        if (_input == null)
            Debug.LogError($"Cannot find input for handle {type}");

        _field = field;
        _matchState = matchState;
        _pauseHandler = pauseHandler;
    }

    private void Update()
    {
        if (_input == null || !_input.IsPressed() || !_matchState.IsRunning() || _pauseHandler.IsPaused())
            return;

        var relativePosition = _input.GetPosition();
        var screenPosition = _field.SafeWidth / 2 * relativePosition;
        var maxOffset = (_field.SafeWidth - size) / 2;
        var position = Mathf.Clamp(screenPosition , - maxOffset, maxOffset);
        MoveTo(position, Time.deltaTime);
    }

    private void MoveTo(float position, float deltaTime)
    {
        var realPosition = transform.position;
        realPosition.x = Mathf.MoveTowards(realPosition.x, position, deltaTime * speed);
        transform.position = realPosition;
    }

    public void SetPosition(Vector3 position) => transform.position = position;
}
