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

    private void Start()
    {
        vel = new Vector2();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        jumps = jumpsMax;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            //Game over, or just hp--?
        }
        else
        {
            jumps = jumpsMax;
        }
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
    }
}
