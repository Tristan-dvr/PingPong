using System;
using Zenject;

public class SaveGameStateHandler : IInitializable, IDisposable
{
    private IStorage _storage;
    private SignalBus _signalBus;

    public SaveGameStateHandler(IStorage storage, SignalBus signalBus)
    {
        _storage = storage;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameLoop.MatchDataChangedSignal>(OnMatchDataChanged);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameLoop.MatchDataChangedSignal>(OnMatchDataChanged);
        _storage.Save();
    }

    private void OnMatchDataChanged(GameLoop.MatchDataChangedSignal data)
    {
        var record = _storage.GetState().maxScore;
        if (data.matchData.playerScore > record)
        {
            _storage.GetState().maxScore = data.matchData.playerScore;
            _storage.Save();
        }
    }
}
