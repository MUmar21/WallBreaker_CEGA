using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Texts Ref")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Menus Ref")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject timeSelectionMenu;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject instructionMenu;

    [Header("Buttons Ref")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button deleteDataButton;
    [SerializeField] private Button nextButton;

    private const string HasSeenInstructionKey = "HasSeenInstruction";

    private void Start()
    {
        if (PlayerPrefs.HasKey(HasSeenInstructionKey))
        {
            instructionMenu.SetActive(false);
        }
        else
        {
            instructionMenu.SetActive(true);
        }
    }


    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        playButton.onClick.AddListener(OnPlayButton);
        replayButton.onClick.AddListener(OnReplayButton);
        deleteDataButton.onClick.AddListener(DeleteGameData);
        nextButton.onClick.AddListener(OnNextButton);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        playButton.onClick.RemoveListener(OnPlayButton);
        replayButton.onClick.RemoveListener(OnReplayButton);
        deleteDataButton.onClick.RemoveListener(DeleteGameData);
        nextButton.onClick.RemoveListener(OnNextButton);
    }

    private void OnGameStateChanged(GameManager.GameStates newGameState)
    {
        DisableAllMenu();

        switch (newGameState)
        {
            case GameManager.GameStates.MainMenu:
                startMenu.SetActive(true);
                break;
            case GameManager.GameStates.TimerMenu:
                timeSelectionMenu.SetActive(true);
                break;
            case GameManager.GameStates.Playing:
                inGameUI.SetActive(true);
                break;
            case GameManager.GameStates.GameOver:
                timerText.text = FormatedTimer(0);
                scoreText.text = $"Score: 0";
                gameOverMenu.SetActive(true);
                break;
        }
    }

    private void DisableAllMenu()
    {
        startMenu.SetActive(false);
        timeSelectionMenu.SetActive(false);
        inGameUI.SetActive(false);
        gameOverMenu.SetActive(false);
    }


    // ==Listeners==
    private void OnPlayButton()
    {
        GameManager.Instance.ChangeState(GameManager.GameStates.TimerMenu);
    }

    private void OnReplayButton()
    {
        GameManager.Instance.ChangeState(GameManager.GameStates.Playing);
    }
    private void DeleteGameData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnNextButton()
    {
        instructionMenu.SetActive(false);
        PlayerPrefs.SetString(HasSeenInstructionKey, "true");
        PlayerPrefs.Save();
    }

    public void SetTimer(int seconds)
    {
        GameManager.Instance.SetGameplayTime(seconds);
        GameManager.Instance.ChangeState(GameManager.GameStates.Playing);
    }

    public void UpdateCountdownUI(int seconds)
    {
        countdownText.gameObject.SetActive(true);

        if (seconds <= 0)
        {
            countdownText.text = "GO";
            StartCoroutine(DisableCountdown());
            return;
        }

        countdownText.text = seconds.ToString();
    }

    private IEnumerator DisableCountdown()
    {
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
        }
    }

    public void UpdateTimerUI(int seconds)
    {
        timerText.text = FormatedTimer(seconds);
    }

    public void UpdateHighscoreText(int score)
    {
        highScoreText.text = $"High Score: {score}";
    }

    private string FormatedTimer(int seconds)
    {
        int mins = seconds / 60;
        int secs = seconds % 60;

        return $"Timer: {mins}:{secs}";
    }

}
