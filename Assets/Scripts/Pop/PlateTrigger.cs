using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrigger : MonoBehaviour
{
    // Trigger area for the goal of Pop
    [SerializeField] Sprite cleanSprite;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PopLevelController.Instance.GoalReached();
            spriteRenderer.sprite = cleanSprite;
        }
    }
}
