using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUIController : MonoBehaviour
{
    // Controls the UI
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI obstaclesText;

    float gameTimer = 0f;

    private static PopUIController instance;
    public static PopUIController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("PopUIController");
                instance = go.GetComponent<PopUIController>();
            }

            return instance;
        }
    }

    public void UpdateObstacleCount(int obstacleCount, int obstacleTotal)
    {
        obstaclesText.text = $"{obstacleCount} / {obstacleTotal}";
    }

    public void OnMenuPressed()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnReplayPressed()
    {
        SceneManager.LoadSceneAsync("BubblePop");
    }

    public void OnNextPressed()
    {
        SceneManager.LoadSceneAsync("BubbleHop");
    }

    private void Start()
    {
        gameTimer = PopLevelController.Instance.GetGameDuration();
    }

    private void Update()
    {
        gameTimer -= Time.deltaTime;

        timerText.text = gameTimer.ToString("00\\:00"); //Just shows seconds lol
    }
}
