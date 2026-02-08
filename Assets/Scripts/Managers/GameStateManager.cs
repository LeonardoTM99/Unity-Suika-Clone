using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public enum GameState
    {
        Menu,
        Preparing,
        Aiming,
        Dropped,
        GameOver
    }

    public GameState currentState { get; private set; }

    public event Action<GameState> OnGameStateChanged;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentState = GameState.Menu;
    }

    public void SetState (GameState newState)
    {
        if (currentState == newState) 
            return;

        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
