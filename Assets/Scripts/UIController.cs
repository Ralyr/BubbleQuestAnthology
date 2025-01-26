using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // Controls UI Common to all games

    //game timer- total time differs between games

    //win and loss panels

    [SerializeField] GameObject gameOverPanel; //just change the text for win/lose
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;

    [SerializeField] TextMeshProUGUI timerText;

    float gameTimer;

    bool isPlaying = true;

    private void Start()
    {
        gameTimer = GameController.Instance.GetMaxTime();
        gameOverPanel.SetActive(false);

        GameController.Instance.StateChange.AddListener(StateChange);
    }

    public void OnMenuPressed()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnReplayPressed()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnGameEnd(bool win)
    {
        gameOverPanel.SetActive(true);

        loseText.SetActive(!win);
        winText.SetActive(win);
    }

    public void StateChange(GameState newState)
    {
        if (newState == GameState.Lose)
        {
            OnGameEnd(false);
            isPlaying = false;
        }
        else if (newState == GameState.Win)
        {
            OnGameEnd(true);
            isPlaying = false;
        }
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        gameTimer -= Time.deltaTime;

        timerText.text = gameTimer.ToString("00\\:00"); //Just shows seconds lol
    }
}
