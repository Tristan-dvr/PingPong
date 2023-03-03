using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameLoop : IInitializable, IDisposable
{
    private IField _field;
    private IBall _ball;
    private SignalBus _signalBus;
    private IRacket _playerRacket, _enemyRacket;
    private GameSettings _settings;
    private ICountdownTimer _countdown;
    private MatchStateHandler _matchState;

    private MatchData _matchData = new MatchData();

    public GameLoop(
        IField field,
        IBall ball,
        [Inject(Id = PlayerType.Player)] IRacket playerRacket,
        [Inject(Id = PlayerType.Enemy)] IRacket enemyRacket,
        SignalBus bus, 
        GameSettings settings, 
        ICountdownTimer countdown, 
        MatchStateHandler matchState)
    {
        _field = field;
        _ball = ball;
        _playerRacket = playerRacket;
        _enemyRacket = enemyRacket;
        _signalBus = bus;
        _settings = settings;
        _countdown = countdown;
        _matchState = matchState;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GoalSignal>(OnGoalScored);
        _signalBus.Subscribe<StartMatchSignal>(StartMatch);

        _signalBus.Fire(new StartMatchSignal());
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GoalSignal>(OnGoalScored);
        _signalBus.Unsubscribe<StartMatchSignal>(StartMatch);
    }

    private void OnGoalScored(GoalSignal signal)
    {
        switch (signal.gateType)
        {
            case PlayerType.Enemy:
                _matchData.enemyScore++;
                break;
            case PlayerType.Player:
                _matchData.playerScore++;
                break;
        }
        OnScoreChanged();
        ResetField();
    }

    private void OnScoreChanged()
    {
        _signalBus.Fire(new MatchDataChangedSignal(_matchData));
    }

    public void StartMatch()
    {
        _matchData = new MatchData();
        OnScoreChanged();
        ResetField();
    }

    private void ResetField()
    {
        _playerRacket.SetPosition(_field.GetPlayerRacketPosition());
        _enemyRacket.SetPosition(_field.GetEnemyRacketPosition());

        _matchState.SetState(MatchStateHandler.State.Countdown);
        ResetBall();
        _countdown.StartCountdown(3, ()=>
        {
            _matchState.SetState(MatchStateHandler.State.Running);
            LaunchBall();
        });
    }

    private void ResetBall()
    {
        _ball.SetVelocity(Vector3.zero, 0);
        _ball.SetPosition(_field.GetBallStartPosition());
    }

    private void LaunchBall()
    {
        var forward = _field.GetForwardDirection();
        var direction = Random.value >= 0.5f ? forward : -forward;
        var angle = Mathf.Lerp(-_settings.maxStartAngle, _settings.maxStartAngle, Random.value);
        _ball.SetVelocity(Quaternion.Euler(0, angle, 0) * direction, _settings.startBallSpeed);
    }

    public struct StartMatchSignal { }

    public struct MatchDataChangedSignal
    {
        public MatchData matchData;

        public MatchDataChangedSignal(MatchData data)
        {
            matchData = data;
        }
    }
}
