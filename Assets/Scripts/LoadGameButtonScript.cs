using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameButtonScript : MonoBehaviour
{
    SaveStateManager _gameStateManager;

    void Start()
    {
        _gameStateManager = FindObjectOfType<SaveStateManager>();

        if (_gameStateManager == null)
        {
            Debug.LogError("Game State Manager was not found!");
        }
    }

    public void OnClick()
    {
        _gameStateManager?.LoadGame();
    }
}
