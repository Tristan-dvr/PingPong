using System;
using UnityEngine;
using Zenject;

public class AIInput : IRacketInput, IInitializable, IDisposable
{
    private IBall _ball;
    private IField _field;
    private SignalBus _signalBus;

    private bool _active = true;

    public AIInput(IBall ball, IField field, SignalBus signalBus)
    {
        _ball = ball;
        _field = field;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<RacketHitSignal>(OnRacketHit);
        _signalBus.Subscribe<GameLoop.StartMatchSignal>(ActivateInput);
        _signalBus.Subscribe<GoalSignal>(ActivateInput);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<RacketHitSignal>(OnRacketHit);
        _signalBus.Unsubscribe<GameLoop.StartMatchSignal>(ActivateInput);
        _signalBus.Unsubscribe<GoalSignal>(ActivateInput);
    }

    private void ActivateInput()
    {
        _active = true;
    }

    private void OnRacketHit(RacketHitSignal data)
    {
        _active = data.player == PlayerType.Player;
    }

    public float GetPosition()
    {
        if (!_active)
            return 0;

        var ballPosition = _ball.GetPosition().x;
        var fieldWidth = _field.Width;
        var fieldCenter = _field.GetBallStartPosition().x;
        return Mathf.Lerp(-1, 1, Mathf.InverseLerp(fieldCenter - fieldWidth, fieldCenter + fieldWidth, ballPosition));
    }

    public bool IsPressed() => true;
}
