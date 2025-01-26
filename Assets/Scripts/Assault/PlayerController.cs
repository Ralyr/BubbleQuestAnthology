using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Controls the player in assault
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject altBulletPrefab;
    [SerializeField] Transform bulletSpawn;

    List<GameObject> bullets;
    int maxBullets = 20;

    int bulletId = 0;

    float bulletDelay = 0f;
    float bulletDelayMax = 0.25f;

    Rigidbody2D rigid;
    Vector2 vel;

    float speed = 1000f;

    Health health;
    int maxHP = 3;

    bool isDead = false;

    float yLimit = 7f;

    bool gotSecret = false;

    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                instance = go.GetComponent<PlayerController>();
            }

            return instance;
        }
    }

    public void ChangeHP (int amount)
    {
        Debug.Log("Player hit");
        health.HP += amount;
    }

    private void Start()
    {
        gotSecret = PlayerPrefs.HasKey("secret") && PlayerPrefs.GetInt("secret") == 1;
        bullets = new List<GameObject>();
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = GameObject.Instantiate(gotSecret ? altBulletPrefab : bulletPrefab, transform.position, Quaternion.identity);
            bullets.Add(bullet);
            bullet.SetActive(false);
        }

        rigid = gameObject.GetComponent<Rigidbody2D>();
        vel = new Vector2(0f, 0f);

        health = gameObject.GetComponent<Health>();
        health.deathEvent.AddListener(Death);
        health.SetMaxHp(maxHP);
    }

    public void Death()
    {
        GameController.Instance.State = GameState.Lose;
        isDead = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            health.HP--;
        }
    }

    private void Update()
    {
        if (isDead)
            return;

        if (Input.GetKey(KeyCode.Space))
        {
            //fire every x seconds
            bulletDelay += Time.deltaTime;
            if (bulletDelay >= bulletDelayMax)
            {
                GameObject bullet = bullets[bulletId];
                //move bullet's position to spawn
                bullet.transform.position = bulletSpawn.position;
                bullet.transform.rotation = gameObject.transform.rotation;
                //activate
                bullet.SetActive(true);
                //increment id
                bulletId++;

                if (bulletId >= maxBullets)
                    bulletId = 0;

                bulletDelay = 0f;
            }
        }

        vel.x = Input.GetAxis("Horizontal");
        vel.y = Input.GetAxis("Vertical");

        rigid.velocity = vel * speed * Time.deltaTime;

        //Wrap around if we go out of bounds in y
        if (transform.position.y > yLimit)
            transform.position = new Vector3(transform.position.x, -yLimit, 0f);
        else if (transform.position.y < -yLimit)
            transform.position = new Vector3(transform.position.x, yLimit, 0f);

    }
}
