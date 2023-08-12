using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    public int maxHealth = 4;
    int health;

    public SpriteRenderer spriteRenderer;
    public PlayerEventChannel playerEventChannel;

    float damageTimer;
    float damageTime = 1f;
    bool canTakeDamage;

    [Header("Effects")]
    public CinemachineImpulseSource impulseSource;
    public ParticleSystem damageVFX;

    public void Start()
    {
        health = maxHealth;
        playerEventChannel.RaiseOnSetPlayerHealth(maxHealth);
        playerEventChannel.playerTransform = transform;
        playerEventChannel.OnRespawn += EnableControls;
    }

    private void OnDestroy()
    {
        playerEventChannel.OnRespawn -= EnableControls;
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

    public void TakeDamage(float damageAmount)
    {
        if (!canTakeDamage)
            return;

        canTakeDamage = false;

        health--;
        spriteRenderer.color = Color.red;

        playerEventChannel.RaiseOnTakeDamage();
        
        Instantiate(damageVFX, transform.position, Quaternion.identity).Play();
        impulseSource.GenerateImpulse();

        if (health <= 0)
        {
            DisableControls();
            playerEventChannel.RaiseOnPlayerDeath();
            Heal(maxHealth);
        }
    }

    void DisableControls()
    {
        GetComponent<PlayerMovementController>().playerInputs.Disable();
    }

    void EnableControls()
    {
        GetComponent<PlayerMovementController>().playerInputs.Enable();
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        playerEventChannel.RaiseOnHeal(healAmount);
    }
}
