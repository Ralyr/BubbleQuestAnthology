using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    // Script to control health and iframes for anything that can be damaged
    new ParticleSystem particleSystem;
    int particleBurstCount = 5;

    int hp;
    int hpMax;
    float iframes = -1f;
    float iframesMax = 1f;

    public UnityEvent deathEvent;
    public bool isActive = true;

    private void Start()
    {
        HP = hpMax;
        particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    public int HP
    {
        get { return hp; }
        set
        {
            if (!isActive)
                return;

            //ToDo: some kind of effect when hp goes down to draw the player's attention
            if (value < hp) //We took damage
            {
                if (iframes >= 0) //Don't take damage while we have iframes
                    return;
                else
                    iframes = 0; //Start the iframes

                particleSystem.Emit(particleBurstCount);
            }
            hp = value;

            if (hp <= 0)
                deathEvent.Invoke();
        }
    }

    public void SetMaxHp(int max)
    {
        hpMax = max;
        hp = hpMax;
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (iframes >= 0)
        {
            iframes += Time.deltaTime;
            if (iframes >= iframesMax)
            {
                iframes = -1f;
            }
        }
    }
}
