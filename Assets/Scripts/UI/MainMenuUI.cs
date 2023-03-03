using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    public Button startButton;

    private SignalBus _signalBus;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartPressed);
    }

    [Inject]
    protected void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnStartPressed()
    {
        _signalBus.Fire(new SceneManagementHandler.LoadSceneSignal("Game"));
    }
}
