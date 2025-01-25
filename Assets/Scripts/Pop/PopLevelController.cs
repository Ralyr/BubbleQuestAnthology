using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopLevelController : MonoBehaviour
{
    // Controls the placement of the obstacles for Bubble Pop

    [SerializeField] GameObject goalPrefab;
    [SerializeField] List<GameObject> obstacles; //keep reusing the same obstacles

    float xSpacing = 5f;
    float currentXPos;
    float goalYPos = 5f; //should be centered

    int currentId = 0;

    float obstacleTimer = -5f; //Give the player a chance to get through a few obstacles first
    float obstacleTimerMax = 2f;

    float gameTimer = 0f;
    float gameTimerMax = 30f; //ToDo: too short?

    //ToDo: count obstacles cleared?

    int obstaclesCleared = 0;
    int obstaclesNeeded = 10;

    private static PopLevelController instance;
    public static PopLevelController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("PopLevelController");
                instance = go.GetComponent<PopLevelController>();
            }

            return instance;
        }
    }

    private void Start()
    {
        //initial randomization of obstacle positions

        //space each one about 4 units apart in x, randomize y between 2 and 8 (5 is the center)

        currentXPos = xSpacing;
        for (int i = 0; i < obstacles.Count; i++)
        {
            UpdateObstacle(i);
        }

        PopUIController.Instance.UpdateObstacleCount(obstaclesCleared, obstaclesNeeded);
    }

    public void PassObstacle()
    {
        obstaclesCleared++;
        Debug.Log($"{obstaclesCleared} obstacles passed");
        PopUIController.Instance.UpdateObstacleCount(obstaclesCleared, obstaclesNeeded);
    }

    public void GoalReached()
    {
        //ToDo: display some sort of victory screen
        PlayerPrefs.SetInt("pop", 1);
    }

    public float GetGameDuration() { return gameTimerMax; }

    private void UpdateObstacle(int id)
    {
        GameObject obstacle = obstacles[id];

        float rand = Random.Range(2f, 8f);
        obstacle.transform.position = new Vector3(currentXPos, rand, 0f);
        currentXPos += xSpacing;
    }

    void SpawnGoal()
    {
        currentXPos += xSpacing; //give the player extra time to hit the goal
        GameObject goal = GameObject.Instantiate(goalPrefab, new Vector3(currentXPos, goalYPos, 0f), Quaternion.identity);
    }

    private void Update()
    {
        //every x seconds take the oldest obstacle and move it forward
        obstacleTimer += Time.deltaTime;
        if (obstacleTimer > obstacleTimerMax)
        {
            //ToDo: after x obstacles cleared, instead of updating the obstacles spawn the goal
            if (obstaclesCleared >= obstaclesNeeded)
            {
                SpawnGoal();
                obstacleTimer = 0f;
                obstacleTimerMax *= 2f; //give player time to hit the goal, spawn another if they miss it.
                return;
            }
            
            UpdateObstacle(currentId);
            currentId++;

            if (currentId >= obstacles.Count)
                currentId = 0;

            obstacleTimer = 0f;
        }

        gameTimer += Time.deltaTime; //ToDo: hook this up to the text object
        if (gameTimer > gameTimerMax)
        {
            //ToDo: Pop the bubble
            Debug.Log("Game Ended");
        }

    }
}
