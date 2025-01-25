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

            state = value;

            StateChange.Invoke(state);
        }
    }

    private void Start()
    {
        StateChange = new GameStateChange();
        gameTimerMax = GetMaxTime();
        State = GameState.Playing;
    }

    public float GetMaxTime()
    {
        return SceneManager.GetActiveScene().name == "BubblePop" ? 30f : 60f;
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        if (gameTimer >= gameTimerMax)
        {
            //game over
            state = GameState.Lose;
        }
    }
}
