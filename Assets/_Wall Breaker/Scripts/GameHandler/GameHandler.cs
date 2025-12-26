using System.Collections;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject levelComplete;
    public TMP_Text scoreText;

    private bool Gameover;
    public int score;

    private void OnEnable()
    {
        BricksHandler.OnDetached += OnBrickDetached;
        BrickWallGenerator.OnLevelComplete += OnLevelCompleted;
    }


    private void OnDisable()
    {
        BricksHandler.OnDetached -= OnBrickDetached;
        BrickWallGenerator.OnLevelComplete -= OnLevelCompleted;
    }

    private void OnDestroy()
    {
        BricksHandler.OnDetached -= OnBrickDetached;
        BrickWallGenerator.OnLevelComplete -= OnLevelCompleted;
    }

    private void OnBrickDetached(BricksHandler bricksHandler)
    {
        Debug.Log("Brick Detached " + bricksHandler.gameObject.name);
        AddScore();
    }

    private void OnLevelCompleted()
    {
        Debug.Log("Level Completed!");
        StartCoroutine(EnableLevelCompleteAfterDelay());
    }

    private IEnumerator EnableLevelCompleteAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        score = 0;
        scoreText.text = "Score: " + score;
        levelComplete.SetActive(true);
    }

    public void AddScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
        Debug.Log("Score: " + score);
    }
}


