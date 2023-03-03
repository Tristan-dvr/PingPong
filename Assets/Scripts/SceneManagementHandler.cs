using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneManagementHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;

    public SceneManagementHandler(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<LoadSceneSignal>(OnLoadSceneRequested);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<LoadSceneSignal>(OnLoadSceneRequested);
    }

    private void OnLoadSceneRequested(LoadSceneSignal data)
    {
        SceneManager.LoadScene(data.sceneName);
        Time.timeScale = 1;
    }

    public struct LoadSceneSignal
    {
        public string sceneName;

        public LoadSceneSignal(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }
}
