using UnityEngine;
using Zenject;

public class UnityEventsHandler : MonoBehaviour
{
    private SignalBus _signalBus;

    [Inject]
    protected void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            _signalBus.Fire(new GamePauseHandler.PauseSignal(true));
    }
}
