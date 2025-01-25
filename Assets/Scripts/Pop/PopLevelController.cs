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
    float previousYPos = -1;
    float yPosMaxChange = 2f;
    float goalOffset = -0.75f; //The difference between the center of the top obstacle, and the gap the player passes through.

    int currentId = 0;

    float obstacleTimer = -5f; //Give the player a chance to get through a few obstacles first
    float obstacleTimerMax = 2f;

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
        GameController.Instance.State = GameState.Win;
    }

    private void UpdateObstacle(int id)
    {
        GameObject obstacle = obstacles[id];
        float rand;
        if (previousYPos == -1)
            rand = Random.Range(2f, 8f); //ToDo: should be random but relative to the previous value, so the player is more likely to be able to hit it?
        else
            rand = Random.Range(previousYPos - yPosMaxChange, previousYPos + yPosMaxChange);

        obstacle.transform.position = new Vector3(currentXPos, rand, 0f);
        currentXPos += xSpacing;
        previousYPos = rand;
    }

    void SpawnGoal()
    {
        currentXPos += xSpacing;
        GameObject goal = GameObject.Instantiate(goalPrefab, new Vector3(currentXPos, previousYPos + goalOffset, 0f), Quaternion.identity);
    }

    private void Update()
    {
        //every x seconds take the oldest obstacle and move it forward
        obstacleTimer += Time.deltaTime;
        if (obstacleTimer > obstacleTimerMax)
        {
            //ToDo: after x obstacles cleared, instead of updating the obstacles spawn the goal
            if (obstaclesCleared >= obstaclesNeeded) //On average, we hit another 4 obstacles after reaching the 10 since they've already been repositioned, maybe do this 4 obstacles earlier?
            {
                SpawnGoal();
                obstacleTimer = 0f;
                return;
            }
            
            UpdateObstacle(currentId);
            currentId++;

            if (currentId >= obstacles.Count)
                currentId = 0;

            obstacleTimer = 0f;
        }
    }
}
