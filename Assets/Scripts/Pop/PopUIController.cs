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

    public void OnNextPressed()
    {
        SceneManager.LoadSceneAsync("BubbleHop");
    }
}
