using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;

    private int currentScore = 0;


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
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }


    public void AddPoints(int points)
    {
        currentScore += points;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }

    #region Game States Logic

    private void HandleGameStateChanged(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.GameOver)
        {
            ResetScore();
        }
    }

    #endregion
}
