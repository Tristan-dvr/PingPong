using System;

public interface ICountdownTimer
{
    void StartCountdown(int seconds, Action onFinished);
}
