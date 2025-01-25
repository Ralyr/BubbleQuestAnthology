using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Control the main menu

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
        SceneManager.LoadSceneAsync("Assault");
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}
