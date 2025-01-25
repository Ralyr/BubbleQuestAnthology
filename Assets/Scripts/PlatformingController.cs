using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingController : MonoBehaviour
{
    // Controls the player in the sidescrolling platformer game

    Vector2 vel;
    Rigidbody2D rigid;

    float speed = 2f;
    float jumpSpeed = 4f;
    //Vector2 jumpVel = new Vector2(0f, 500f);

    private void Start()
    {
        vel = new Vector2();
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //vel = Vector2.zero;
        vel.x = Input.GetAxis("Horizontal");
        vel.y = Input.GetAxis("Vertical"); //ToDo: remove this if it's not helpful

        vel.x *= speed;

        if (Input.GetKey(KeyCode.Space))
        {
            vel.y += jumpSpeed;
        }

        rigid.velocity = vel;
         
        /*
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rigid.AddForce(jumpVel);
        }*/
    }
}
