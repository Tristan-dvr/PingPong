using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour, ICountdownTimer, IInitializable, IDisposable
{
    [Header("Pause")]
    public GameObject pauseUI;
    public Button continueButton;
    public Button restartButton;
    public Button mainMenuButton;
    [Header("Score")]
    public GameObject scoreUi;
    public Button pauseButton;
    [Space]
    public Text playerScoreText;
    public Text maxScoreText;
    public Text enemyScoreText;
    public Text countdownText;

    private SignalBus _signalBus;
    private IStorage _storage;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinuePressed);
        restartButton.onClick.AddListener(OnRestartButtonPressed);
        pauseButton.onClick.AddListener(OnPauseButtonPressed);
        mainMenuButton.onClick.AddListener(OnMainMenuPressed);
    }

    [Inject]
    protected void Construct(SignalBus signalBus, IStorage storage)
    {
        _signalBus = signalBus;
        _storage = storage;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameLoop.MatchDataChangedSignal>(OnMatchDataChanged);
        _signalBus.Subscribe<GamePauseHandler.PauseSignal>(OnPause);

        pauseUI.SetActive(false);
        scoreUi.SetActive(true);
        RefreshMaxScoreText();
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameLoop.MatchDataChangedSignal>(OnMatchDataChanged);
        _signalBus.Unsubscribe<GamePauseHandler.PauseSignal>(OnPause);
    }

    private void RefreshMaxScoreText()
    {
        maxScoreText.text = _storage.GetState().maxScore.ToString();
    }

    private void OnMainMenuPressed()
    {
        _signalBus.Fire(new SceneManagementHandler.LoadSceneSignal("Menu"));
    }

    public void OnContinuePressed()
    {
        _signalBus.Fire(new GamePauseHandler.PauseSignal(false));
    }

    public void OnPauseButtonPressed()
    {
        _signalBus.Fire(new GamePauseHandler.PauseSignal(true));
    }

    public void OnRestartButtonPressed()
    {
        _signalBus.Fire(new GamePauseHandler.PauseSignal(false));
        _signalBus.Fire(new GameLoop.StartMatchSignal());
    }

    private void OnPause(GamePauseHandler.PauseSignal data)
    {
        pauseUI.SetActive(data.pause);
    }

    private void OnMatchDataChanged(GameLoop.MatchDataChangedSignal data)
    {
        playerScoreText.text = data.matchData.playerScore.ToString();
        enemyScoreText.text = data.matchData.enemyScore.ToString();
        RefreshMaxScoreText();
    }

    public void StartCountdown(int seconds, Action onFinished)
    {
        StopAllCoroutines();
        StartCoroutine(CountdownCoroutine(seconds, onFinished));
    }

    private IEnumerator CountdownCoroutine(int seconds, Action onFinished)
    {
        var timer = seconds;
        var delay = new WaitForSeconds(1);
        countdownText.CrossFadeAlpha(1, 0, true);
        while (timer > 0)
        {
            countdownText.text = timer.ToString();
            yield return delay;
            timer--;
        }
        countdownText.text = timer.ToString();
        countdownText.CrossFadeAlpha(0, 0.5f, true);
        onFinished?.Invoke();
    }
}
