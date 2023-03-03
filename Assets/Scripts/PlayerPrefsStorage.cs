using System;
using UnityEngine;

public class PlayerPrefsStorage : IStorage
{
    private SavedGameState _state;

    public PlayerPrefsStorage()
    {
        try
        {
            var json = PlayerPrefs.GetString("game_state");
            if (string.IsNullOrEmpty(json))
                _state = new SavedGameState();
            else
                _state = JsonUtility.FromJson<SavedGameState>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while loading game state: {e.Message}\n{e.StackTrace}");
            _state = new SavedGameState();
        }
    }

    public SavedGameState GetState() => _state;

    public void Save()
    {
        PlayerPrefs.SetString("game_state", JsonUtility.ToJson(_state));
    }
}
