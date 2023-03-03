public class MatchStateHandler
{
    public enum State
    {
        Countdown,
        Running,
    }

    private State _state;

    public void SetState(State state)
    {
        _state = state;
    }

    public State GetState() => _state;

    public bool IsRunning() => _state == State.Running;
}
