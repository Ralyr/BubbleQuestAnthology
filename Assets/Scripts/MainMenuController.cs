using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Control the main menu
    [SerializeField] GameObject hopButton;
    [SerializeField] GameObject assaultButton;
    [SerializeField] GameObject secret;

    int pop = 0;
    int hop = 0;

    private void Start()
    {
        hopButton.SetActive(false);
        assaultButton.SetActive(false);
        secret.SetActive(false);

        if (PlayerPrefs.HasKey("pop"))
        {
            pop = PlayerPrefs.GetInt("pop");
        }
        else
        {
            PlayerPrefs.SetInt("pop", pop);
        }

        if (pop != 0)
        {
            hopButton.SetActive(true);
        }

        if (PlayerPrefs.HasKey("hop"))
        {
            hop = PlayerPrefs.GetInt("hop");
        }
        else
        {
            PlayerPrefs.SetInt("hop", 0);
        }

        if (hop != 0)
        {
            assaultButton.SetActive(true);
        }

        if (PlayerPrefs.HasKey("assault"))
        {
            if (PlayerPrefs.GetInt("assault") == 1)
            {
                secret.SetActive(true);
            }
        }
    }
    public void OnGame1Press()
    {
        SceneManager.LoadSceneAsync("BubblePop");
    }

    public void OnGame2Press()
    {
        SceneManager.LoadSceneAsync("BubbleHop");
    }

    public void OnGame3Press()
    {
        SceneManager.LoadSceneAsync("BubbleAssault");
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}
