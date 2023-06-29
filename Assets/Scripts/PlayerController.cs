using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 5;
    int health = 5;

    public SpriteRenderer spriteRenderer;
    public PlayerEventChannel playerEventChannel;

    float damageTimer;
    float damageTime = 1f;
    bool canTakeDamage;

    public void Start()
    {
        playerEventChannel.RaiseOnSetPlayerHealth(maxHealth);
    }

    private void Update()
    {
        InvinsibilityFrames();
    }

    public void InvinsibilityFrames()
    {
        if (!canTakeDamage)
        {
            if (damageTimer < damageTime)
            {
                damageTimer += Time.deltaTime;
            }
            else
            {
                canTakeDamage = true;
                damageTimer = 0;
                spriteRenderer.color = Color.white;
            }
        }
    }

    public void TakeDamage()
    {
        if (!canTakeDamage)
            return;

        canTakeDamage = false;

        health--;
        spriteRenderer.color = Color.red;

        playerEventChannel.RaiseOnTakeDamage();

        if (health <= 0)
        {
            playerEventChannel.RaiseOnPlayerRespawn(transform);
            Heal(maxHealth);
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        playerEventChannel.RaiseOnHeal(healAmount);
    }
}
