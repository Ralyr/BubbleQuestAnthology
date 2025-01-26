using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    // Boss arm
    [SerializeField] Transform bulletSpawn;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Sprite deadSprite;
    [SerializeField] float bulletOffset;
    [SerializeField] BossHead head;

    List<GameObject> bullets;
    int bulletsMax = 10;
    int bulletId = 0;

    int burstCountMax = 5;

    float bulletDelay = 0f;
    float bulletDelayMax = 3f;

    SpriteRenderer spriteRenderer;
    new Collider2D collider;

    Health health;
    int maxHP = 10;

    bool isDead = false;

    private void Start()
    {
        health = gameObject.GetComponent<Health>();
        health.SetMaxHp(maxHP);
        health.deathEvent.AddListener(Death);
        bullets = new List<GameObject>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<Collider2D>();

        for (int i = 0; i < bulletsMax; i++)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullets.Add(bullet);
            bullet.SetActive(false);
        }
    }

    public void ChangeHP(int amount)
    {
        health.HP += amount;
    }

    public void Death()
    {
        //spriteRenderer.sprite = deadSprite; //ToDo: Uncomment when art
        isDead = true;
        collider.enabled = false;
        head.ArmDead();
    }

    private void Update()
    {
        if (isDead)
            return;

        //Fires a spread of bullets
        bulletDelay += Time.deltaTime;
        if (bulletDelay >= bulletDelayMax)
        {
            bulletDelay = 0f;

            GameObject bullet;
            Vector3 bulletPos = bulletSpawn.position;
            bulletPos.y -= bulletOffset * 2f;
            for (int i = 0; i < burstCountMax; i++)
            {
                //place each bullet and activate them
                bullet = bullets[bulletId];
                bullet.transform.position = bulletPos;
                bulletPos.y += bulletOffset;
                bullet.SetActive(true);

                bulletId++;
                if (bulletId >= bulletsMax)
                    bulletId = 0;

            }
        }
    }
}
