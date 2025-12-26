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
    [SerializeField] private int scoreAmount = 5;
    [SerializeField] private float gameOverDelay = 3f;
    private int score = 0;


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
        BrickWallGenerator.OnLevelComplete += OnLevelCompleted;
    }

    private void OnDestroy()
    {
        BricksHandler.AddScoreEvent -= AddScore;
        BrickWallGenerator.OnLevelComplete -= OnLevelCompleted;
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
                brickWallGenerator.GenerateWall();
                break;
            case GameStates.GameOver:
                //Time.timeScale = 0.00001f;
                break;

        }
    }

    private void OnLevelCompleted()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        Debug.Log("Game Over");
        yield return new WaitForSeconds(gameOverDelay);
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

}
