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

    public int hp;
    int hpMax = 3;

    float iframes = -1f;
    float iframesMax = 1f;

    float minYPos = -10f;

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
        HP = hpMax;
        jumps = jumpsMax;
    }

    public int HP
    {
        get { return hp; }
        set
        {
            //ToDo: some kind of effect when hp goes down to draw the player's attention
            if (value < hp) //We took damage
            {
                if (iframes >= 0) //Don't take damage while we have iframes
                    return;
                else
                    iframes = 0; //Start the iframes
            }

            hp = value;

            if (hp <= 0)
            {
                Debug.Log("Hp <= 0");
                GameController.Instance.State = GameState.Lose;
            }
            else if (hp == 1)
            {
                jumpsMax = 3;
            }
            else if (hp == 2)
            {
                jumpsMax = 2;
            }
            else if (hp >= 3)
            {
                jumpsMax = 1;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            HP--;
        }

        //Regain jumps even if we took damage, so there's risk-reward to hitting hazards.
        jumps = jumpsMax;
    }

    private void Update()
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

        if (iframes >= 0)
        {
            iframes += Time.deltaTime;
            if (iframes >= iframesMax)
            {
                iframes = -1f;
            }
        }
    }
}
