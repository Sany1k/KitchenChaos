using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    public static KitchenGameManager Instance { get; private set; }

    public event Action OnStateChanged;
    public event Action OnGamePaused;
    public event Action OnGameUnpaused;

    private GameState state;
    private float countdownToStartTimer = 3f;
    private readonly float gamePlayingTimerMax = 120f;
    private float gamePlayingTimer;
    private bool isGamePaused;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        state = GameState.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += PauseAction;
        GameInput.Instance.OnInteractAction += InteractAction;
        GameOverUI.Instance.OnGameRestartEvent += GameRestartEvent;
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.WaitingToStart:
                break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    state = GameState.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke();
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
                {
                    state = GameState.GameOver;
                    OnStateChanged?.Invoke();
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    public bool IsGamePlaying() => state == GameState.GamePlaying;

    public bool IsCountdownToStartActive() => state == GameState.CountdownToStart;

    public bool IsGameOver() => state == GameState.GameOver;

    public float GetCountdownToStartTimer() => countdownToStartTimer;

    public float GetGamePlayingTimerNormalized() => 1 - (gamePlayingTimer / gamePlayingTimerMax);

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            OnGamePaused?.Invoke();
            Time.timeScale = 0f;
        }
        else
        {
            OnGameUnpaused?.Invoke();
            Time.timeScale = 1f;
        }
    }

    private void PauseAction()
    {
        TogglePauseGame();
    }

    private void InteractAction()
    {
        if (state == GameState.WaitingToStart)
        {
            state = GameState.CountdownToStart;
            OnStateChanged?.Invoke();
        }
    }

    private void GameRestartEvent()
    {
        Loader.LoadScene(Loader.Scene.GameScene);
    }
}
