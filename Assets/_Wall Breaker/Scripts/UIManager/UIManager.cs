using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button playButton;
    [SerializeField] private Button replayButton;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        playButton.onClick.AddListener(OnPlayButton);
        replayButton.onClick.AddListener(OnReplayButton);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        playButton.onClick.RemoveListener(OnPlayButton);
        replayButton.onClick.RemoveListener(OnReplayButton);
    }

    private void OnGameStateChanged(GameManager.GameStates newGameState)
    {
        DisableAllMenu();

        switch (newGameState)
        {
            case GameManager.GameStates.MainMenu:
                startMenu.SetActive(true);
                break;
            case GameManager.GameStates.Playing:
                inGameUI.SetActive(true);
                break;
            case GameManager.GameStates.GameOver:
                gameOverMenu.SetActive(true);
                break;
        }
    }

    private void DisableAllMenu()
    {
        startMenu.SetActive(false);
        inGameUI.SetActive(false);
        gameOverMenu.SetActive(false);
    }


    // ==Listeners==
    private void OnPlayButton()
    {
        GameManager.Instance.ChangeState(GameManager.GameStates.Playing);
    }

    private void OnReplayButton()
    {
        GameManager.Instance.ChangeState(GameManager.GameStates.Playing);
    }

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
        }
    }

}
