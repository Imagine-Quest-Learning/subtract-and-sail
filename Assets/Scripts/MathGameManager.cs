using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MathGameManager : MonoBehaviour
{
    public GameObject mathPanel;
    public GameObject gameOverPanel;
    public Text questionText;
    public InputField answerInput;
    public Text scoreText;
    public Text finalScore;

    public Image[] heartSprites;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool isMathActive = false;
    public int minMathInterval = 5;
    public int maxMathInterval = 12;
    private int score = 0;
    public int playerHearts = 3;
    private int correctAnswer;

    void Start()
    {
        // Make sure the math panel is initially not visible
        mathPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        StartCoroutine(ScoreIncrement());
        StartCoroutine(MathQuestionTimer());
        UpdateUI();

        answerInput.onEndEdit.AddListener(delegate { CheckAnswerOnEnter(); });
    }

    IEnumerator MathQuestionTimer()
    {
        while (playerHearts > 0)
        {
            // Show question after a random amount of time
            yield return new WaitForSeconds(Random.Range(minMathInterval, maxMathInterval));

            if (playerHearts > 0 && !isMathActive)
            {
                ShowMathQuestion();
            }
        }
    }

    void ShowMathQuestion()
    {
        isMathActive = true;
        mathPanel.SetActive(true);
        GenerateMathProblem();
        answerInput.Select();
    }

    void GenerateMathProblem()
    {
        // Generate problem with these parameters (first number from 1-19)
        int a = Random.Range(1, 20);
        int b = Random.Range(1, a);
        correctAnswer = a - b;

        // Show question
        questionText.text = a + " - " + b + "?";
    }

    public void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                score += 10;
                ResetMathGame();
            }
            else
            {
                playerHearts--;
                UpdateUI();

                if (playerHearts <= 0)
                {
                    GameOver();
                }
            }
        }
        // Reset question input field after submitting
        answerInput.text = "";
    }

    void CheckAnswerOnEnter()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
    }


    public void PlayerHit()
    {
        // Player loses heart when hit by enemy
        playerHearts--;
        UpdateUI();

        if (playerHearts <= 0)
        {
            GameOver();
        }
    }

    void ResetMathGame()
    {
        Time.timeScale = 1;
        answerInput.text = "";
        mathPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        isMathActive = false;
        UpdateUI();
    }

    void GameOver()
    {
        finalScore.text = scoreText.text;
        mathPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        isMathActive = false;
        Time.timeScale = 0;
    }

    IEnumerator ScoreIncrement()
    {
        // Increase score while game is ongoing
        while (playerHearts > 0)
        {
            yield return new WaitForSeconds(1);
            score++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        scoreText.text = $"{score}";

        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (i < playerHearts)
                heartSprites[i].sprite = fullHeart;
            else
                heartSprites[i].sprite = emptyHeart;
        }
    }

    public void MainMenu()
    {
        ResetMathGame();
        SceneManager.LoadSceneAsync(1);
    }

    public void Restart()
    {
        ResetMathGame();
        SceneManager.LoadSceneAsync(0);
    }

}
