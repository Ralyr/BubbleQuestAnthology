using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopController : MonoBehaviour
{
    // Controls the player in the flappyBird game

    Vector2 vel;
    Rigidbody2D rigid;

    float speed = 3f; //ToDo: speed up over time?
    Vector2 jumpSpeed = new Vector2(0f, 10f);

    GameState state;

    private void Start()
    {
        vel = new Vector2();
        rigid = gameObject.GetComponent<Rigidbody2D>();

        GameController.Instance.StateChange.AddListener(StateChange);
    }

    public void StateChange(GameState newState) { state = newState; }

    private void Update()
    {
        if (state != GameState.Playing)
            return;

        vel.x = speed;
        rigid.velocity = vel;

        if (Input.GetKey(KeyCode.W))
        {
            rigid.AddForce(jumpSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rigid.AddForce(-jumpSpeed); //ToDo: should this be slower?
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("POP");
        GameController.Instance.State = GameState.Lose;
    }
}
