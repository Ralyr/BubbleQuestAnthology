using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameStateChange : UnityEvent<GameState> { }
public enum GameState { Playing, Lose, Win}
public class GameController : MonoBehaviour
{
    // Controls the game state

    //Get the max timer value based on the current scene
    //keep track of game win/loss

    float gameTimer = 0f;
    float gameTimerMax;

    GameState state;

    public GameStateChange StateChange;

    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("GameController");
                instance = go.GetComponent<GameController>();
            }

            return instance;
        }
    }

    public GameState State
    {
        get { return state; }
        set
        {
            if (state == value)
            {
                Debug.Log($"state is already {value}");
                return;
            }

            if (state == GameState.Win) //Don't let the state switch to lose if the player has already won. If they win posthumusly that's fine though.
                return;

            state = value;
            Debug.Log($"State set to {state}");

            if (state == GameState.Win)
            {
                if (SceneManager.GetActiveScene().name == "BubblePop")
                {
                    PlayerPrefs.SetInt("pop", 1);
                }
                else if (SceneManager.GetActiveScene().name == "BubbleHop")
                {
                    PlayerPrefs.SetInt("hop", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("assault", 1);
                    //ToDo: do something special for beating all 3 games
                }
            }

            StateChange.Invoke(state);
        }
    }

    private void Awake()
    {
        StateChange = new GameStateChange();
        gameTimerMax = GetMaxTime();
        State = GameState.Playing;
    }

    public float GetMaxTime()
    {
        float time = 0;
        switch (SceneManager.GetActiveScene().name)
        {
            case "BubblePop":
                time = 30f;
                break;
            case "BubbleHop":
                time = 60f;
                break;
            case "BubbleAssault":
                time = 90f;
                break;
        }

        return time;
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        if (gameTimer >= gameTimerMax)
        {
            //game over
            State = GameState.Lose;
        }
    }
}
