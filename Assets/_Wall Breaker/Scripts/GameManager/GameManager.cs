using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameManager instance;

    public enum GameStates
    {
        MainMenu,
        Playing,
        GameOver
    }

    public GameStates currentState = GameStates.MainMenu;

    public event Action<GameStates> OnGameStateChanged;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private BrickWallGenerator brickWallGenerator;
    [SerializeField] private int gameplayTimeInSeconds = 60;
    [SerializeField] private int scoreAmount = 5;
    [SerializeField] private float gameOverDelay = 3f;

    private int score = 0;

    private int remaingTime;

    private Coroutine countDownRoutine;
    private Coroutine timerCoroutine;
    private Coroutine gameOverCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            instance = Instance;
            Debug.Log($"Active Game Manager Instance: {instance.gameObject.name}");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Game Manager Instance Already exist Destroying this instance: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        BricksHandler.AddScoreEvent += AddScore;
    }


    public void ChangeState(GameStates newState)
    {
        GameStates previousState = currentState;
        Debug.Log($"Previous Game State: {previousState}, New Game State: {newState}");
        currentState = newState;
        OnGameStateChanged?.Invoke(currentState);
        CheckState(currentState);
    }

    private void CheckState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.MainMenu:
                Time.timeScale = 0.00001f;
                break;
            case GameStates.Playing:
                Time.timeScale = 1;
                brickWallGenerator.DestroyCurrentWall();
                StartCountdown();
                break;
            case GameStates.GameOver:
                score = 0;
                remaingTime = 0;
                Time.timeScale = 0.00001f;
                break;

        }
    }

    private void StartCountdown()
    {
        if (countDownRoutine != null)
        {
            StopCoroutine(countDownRoutine);
            countDownRoutine = null;
        }

        countDownRoutine = StartCoroutine(CountdownCoroutine());
    }

    private void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void GameOver()
    {
        if (gameOverCoroutine != null)
        {
            StopCoroutine(gameOverCoroutine);
            gameOverCoroutine = null;
        }
        gameOverCoroutine = StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        int countdown = 3;

        while (countdown >= 0)
        {
            uiManager.UpdateCountdownUI(countdown);
            yield return new WaitForSecondsRealtime(1f);
            countdown--;
        }

        brickWallGenerator.GenerateWall();
        StartTimer();
        countDownRoutine = null;
    }

    private IEnumerator TimerCoroutine()
    {
        remaingTime = gameplayTimeInSeconds;

        while (remaingTime > 0)
        {
            uiManager.UpdateTimerUI(remaingTime);
            remaingTime--;
            yield return new WaitForSeconds(1f);
        }

        if (remaingTime <= 0)
        {
            remaingTime = 0;
            GameOver();
        }
    }

    private IEnumerator GameOverCoroutine()
    {
        Debug.Log("Game Over");
        yield return new WaitForSecondsRealtime(gameOverDelay);
        ChangeState(GameStates.GameOver);
    }

    private void AddScore()
    {
        score += scoreAmount;

        if (uiManager != null)
        {
            uiManager.UpdateScoreUI(score);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        BricksHandler.AddScoreEvent -= AddScore;
    }
}
