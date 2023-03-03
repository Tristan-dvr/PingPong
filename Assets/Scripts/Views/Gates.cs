using UnityEngine;
using Zenject;

public class Gates : MonoBehaviour
{
    public PlayerType player;

    private SignalBus _signalBus;

    [Inject]
    protected void Construct(SignalBus bus)
    {
        _signalBus = bus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent<Ball>(out _))
            _signalBus.Fire(new GoalSignal(player));
    }
}
