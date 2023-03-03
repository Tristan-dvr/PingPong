using System;
using UnityEngine;
using Zenject;

public class GamePauseHandler : IInitializable, IDisposable
{
    private bool _paused;
    private SignalBus _signalBus;

    public GamePauseHandler(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<PauseSignal>(OnPause);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<PauseSignal>(OnPause);
    }

    private void OnPause(PauseSignal data)
    {
        _paused = data.pause;
        Time.timeScale = _paused ? 0 : 1;
    }

    public bool IsPaused() => _paused;

    public struct PauseSignal
    {
        public bool pause;

        public PauseSignal(bool pause)
        {
            this.pause = pause;
        }
    }
}
