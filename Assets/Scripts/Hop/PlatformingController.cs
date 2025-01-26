using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingController : MonoBehaviour
{
    // Controls the player in the sidescrolling platformer game

    Vector2 vel;
    Rigidbody2D rigid;

    float speed = 2f;
    float downSpeed = 0.01f;
    Vector2 jumpVel = new Vector2(0f, 500f);

    float vertSpeedMax = 10f; //ToDo: TEST

    public int jumps; //ToDo: Just public for debugging
    int jumpsMax = 3; //ToDo: number of jumps inverse of remaining health? less health = more jumps?

    Health health;
    int maxHP = 3;

    float minYPos = -10f;

    bool isPlaying = true;

    private static PlatformingController instance;
    public static PlatformingController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                instance = go.GetComponent<PlatformingController>();
            }

            return instance;
        }
    }

    private void Start()
    {
        vel = new Vector2();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        health = gameObject.GetComponent<Health>();
        health.SetMaxHp(maxHP);
        jumps = jumpsMax;

        GameController.Instance.StateChange.AddListener(StateChange);
    }

    public void StateChange(GameState state)
    {
        if (state != GameState.Playing)
            isPlaying = false;
    }
    public void ChangeHP(int amount)
    {
        health.HP += amount;
        if (health.HP <= 0)
        {
            Debug.Log("Hp <= 0");
            GameController.Instance.State = GameState.Lose;
        }
        else if (health.HP == 1)
        {
            jumpsMax = 3;
        }
        else if (health.HP == 2)
        {
            jumpsMax = 2;
        }
        else if (health.HP >= 3)
        {
            jumpsMax = 1;
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            ChangeHP(-1);
        }

        //Regain jumps even if we took damage, so there's risk-reward to hitting hazards.
        jumps = jumpsMax;
    }

    private void Update()
    {
        

        if (isPlaying) //Keep moving the character but stop accepting input on win or lose.
        {
            vel.x = Input.GetAxis("Horizontal");
            vel.y = rigid.velocity.y;

            vel.x *= speed;

            if (jumps > 0 && Input.GetKeyUp(KeyCode.Space))
            {
                rigid.AddForce(jumpVel, ForceMode2D.Force);
                jumps--;
            }

            if (Input.GetKey(KeyCode.S))
            {
                vel.y -= downSpeed;
            }
        }

        //Clamp vel.y within reasonable bounds
        if (vel.y > vertSpeedMax)
            vel.y = vertSpeedMax;
        else if (vel.y < -vertSpeedMax)
            vel.y = -vertSpeedMax;

        rigid.velocity = vel;

        if (transform.position.y <= minYPos)
        {
            transform.position = new Vector3(transform.position.x, -minYPos, 0f); //If we fall below the stage, wrap back to the top
        }
    }
}
