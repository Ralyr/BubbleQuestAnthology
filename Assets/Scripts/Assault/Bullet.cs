using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Controls a bullet.

    [SerializeField] float speed;

    float timer = 0f;
    [SerializeField] float timerMax = 5f;

    bool isPlayerBullet = false;

    private void Start()
    {
        if (gameObject.tag != "Hazard")
            isPlayerBullet = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collision is with something with a health script attached, damage it (do we want friendly fire?)
        if (isPlayerBullet)
        {
            //check if we hit a boss part, damage that part
            if (collision.gameObject.tag == "BossNose")
            {
                collision.gameObject.GetComponent<BossNose>().ChangeHP(-1);
            }
            else if (collision.gameObject.tag == "BossArm")
            {
                collision.gameObject.GetComponent<BossArm>().ChangeHP(-1);
            }
            else if (collision.gameObject.tag == "BossHead")
            {
                collision.gameObject.GetComponent<BossHead>().ChangeHP(-1);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player")
                PlayerController.Instance.ChangeHP(-1);
        }

        //ToDo: explosion effect
        gameObject.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
