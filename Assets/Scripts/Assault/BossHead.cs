using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    // Activates when the other 2 boss components are dead
    [SerializeField] Transform laserStart;
    [SerializeField] BossLaserEnd laserEnd;
    [SerializeField] Sprite deadSprite;

    SpriteRenderer spriteRenderer;

    bool armDead = false;
    bool noseDead = false;

    Health health;
    int maxHp = 10;

    bool isActive = false;
    bool isFiring = false;

    Vector2 laserStartPos;

    float laserTimer = 0f;
    float laserTimerMax = 4f;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        health = gameObject.GetComponent<Health>();
        health.deathEvent.AddListener(Death);
        health.SetMaxHp(maxHp);
        health.isActive = false;

        laserStartPos = new Vector2(laserStart.position.x, laserStart.position.y);
        laserEnd.SetHead(this);
    }

    public void ChangeHP(int amount)
    {
        health.HP += amount;
    }

    public void Death()
    {
        //Head died, player wins!
        isActive = false;
        isFiring = false;

        spriteRenderer.sprite = deadSprite;
        GameController.Instance.State = GameState.Win;
    }

    public void ArmDead()
    {
        armDead = true;
        if (noseDead)
            Activate();
    }

    public void NoseDead()
    {
        noseDead = true;
        if (armDead)
            Activate();
    }

    public void LaserMoveDone()
    {
        isFiring = false;
    }

    void Activate()
    {
        //Time for lasers
        health.isActive = true;
        isFiring = true;
        isActive = true;
        laserEnd.StartMove();
    }

    public Vector3 GetLaserStart() { return laserStart.position; }
    public void EndLaser() { isFiring = false; }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
            Activate();

        if (!isActive)
            return;

        if (isFiring)
        {
            Vector3 endPos = laserEnd.GetPosition();
            RaycastHit2D hit = Physics2D.Linecast(laserStartPos, new Vector2(endPos.x, endPos.y), (1<<6));
            
            if (hit.collider != null)
            {
                //We hit the player, it's the only thing on layer 6
                Debug.Log($"Hit {hit.collider.name}");
                PlayerController.Instance.ChangeHP(-1);
                isFiring = false;
            }
        }
        else
        {
            laserTimer += Time.deltaTime;
            if (laserTimer >= laserTimerMax)
            {
                laserEnd.StartMove();
                isFiring = true;
                laserTimer = 0f;
            }
        }

        
    }
}
