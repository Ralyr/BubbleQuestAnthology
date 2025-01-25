using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    // Trigger area for a goal
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
            GameController.Instance.State = GameState.Win;
            spriteRenderer.sprite = cleanSprite;
        }
    }
}
