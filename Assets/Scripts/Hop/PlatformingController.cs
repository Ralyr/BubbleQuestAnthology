using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingController : MonoBehaviour
{
    // Controls the player in the sidescrolling platformer game

    Vector2 vel;
    Rigidbody2D rigid;

    float speed = 2f;
    //float jumpSpeed = 4f;
    Vector2 jumpVel = new Vector2(0f, 500f);

    public int jumps; //ToDo: Just public for debugging
    int jumpsMax = 3; //??

    private void Start()
    {
        vel = new Vector2();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        jumps = jumpsMax;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard") //ToDo: MAKE TAG
        {
            //Pop, game over
        }
        else
        {
            jumps = jumpsMax;
        }
    }

    private void Update()
    {
        //vel = Vector2.zero;
        vel.x = Input.GetAxis("Horizontal");
        //vel.y = Input.GetAxis("Vertical"); //ToDo: re-add the optional downward speed up, it was nice with such a floaty character and a time limit
        vel.y = rigid.velocity.y;

        vel.x *= speed;

        if (jumps > 0 && Input.GetKeyUp(KeyCode.Space))
        {
            rigid.AddForce(jumpVel, ForceMode2D.Force);
            jumps--;
        }

        rigid.velocity = vel;
    }
}
