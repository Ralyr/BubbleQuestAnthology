using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNose : MonoBehaviour
{
    //Script controling the nose part of the boss
    [SerializeField] Transform bulletSpawn;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Sprite deadSprite;
    [SerializeField] BossHead head;

    SpriteRenderer spriteRenderer;

    List<GameObject> bullets;
    int maxBullets = 20;

    float bulletDelay = 0f;
    float bulletDelayMax = 0.5f;
    int bulletId = 0;
    int burstCount = 0;
    int burstCountMax = 3;

    Health health;
    int maxHP = 10;

    bool isDead = false;

    private void Start()
    {
        bullets = new List<GameObject>();
        for(int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullets.Add(bullet);
            bullet.SetActive(false);
        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        health = gameObject.GetComponent<Health>();
        health.SetMaxHp(maxHP);
        health.deathEvent.AddListener(Death);
    }

    public void ChangeHP(int amount)
    {
        health.HP += amount;
    }

    public void Death()
    {
        spriteRenderer.sprite = deadSprite;
        isDead = true;
        health.isActive = false;
        head.NoseDead();
    }

    private void Update()
    {
        if (isDead)
            return;
        //every x seconds, fire a bullet - just fireing them in a straight line isn't much of a threat, spawn them in a wave with breaks in it or something
        bulletDelay += Time.deltaTime;
        if (bulletDelay >= bulletDelayMax)
        {
            bulletDelay = 0f;
            bullets[bulletId].transform.position = bulletSpawn.position;
            bullets[bulletId].SetActive(true);
            bulletId++;
            if (bulletId >= maxBullets)
                bulletId = 0;

            burstCount++;
            if (burstCount >= burstCountMax)
            {
                burstCount = 0;
                bulletDelay = -bulletDelayMax * 2f;
            }
        }
    }
}
