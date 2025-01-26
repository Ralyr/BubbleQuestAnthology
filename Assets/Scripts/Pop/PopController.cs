using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopController : MonoBehaviour
{
    // Controls the player in the flappyBird game
    SpriteRenderer spriteRenderer;
    ParticleSystem particleSystem;
    Vector2 vel;
    Rigidbody2D rigid;

    float speed = 3f;
    Vector2 jumpSpeed = new Vector2(0f, 3300f);

    GameState state;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        particleSystem = gameObject.GetComponent<ParticleSystem>();
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
            rigid.AddForce(jumpSpeed * Time.deltaTime, ForceMode2D.Force);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rigid.AddForce(-jumpSpeed * Time.deltaTime, ForceMode2D.Force);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        particleSystem.Emit(5);
        spriteRenderer.enabled = false;
        GameController.Instance.State = GameState.Lose;
    }
}
