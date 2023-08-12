using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using DG.Tweening;

public class Entity: MonoBehaviour, IDamagable
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public Rigidbody2D entityRigidbody;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Checkers")]
    public Transform wallCheck;
    public Transform ledgeCheck;
    public Transform playerCheck;
    public Transform groundCheck;

    [Header("Movement Settings")]
    public bool isAffectedByGravity;
    public float gravity = -12;
    public float accelerationTimeGrounded = 0.05f;

    [Header("Player Data")]
    public PlayerRuntimeDataSO playerRuntimeData;

    private Vector2 velocity;
    private float velocityXSmoothing = 0;
    private float health;

    [HideInInspector]
    public int facingDirection = -1;

    public virtual void Start()
    {
        health = (int)entityData.maxHealth;
        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();

        if (isAffectedByGravity)
        {
            float velocityY = CheckGround() ? 0 : entityRigidbody.velocity.y + gravity * Time.deltaTime;
            SetVelocityY(velocityY);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        //StartCoroutine(DamageFlash());
        spriteRenderer.DOColor(Color.red, damageFlashDuration).OnComplete(() =>
        {
            spriteRenderer.DOColor(Color.white, damageFlashDuration);
        });

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    float currentFadeTime;
    public float damageFlashDuration = 0.1f;

    IEnumerator DamageFlash()
    {
        while (currentFadeTime < 1.0f)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.red, Mathf.SmoothStep(0.0f, 1.0f, currentFadeTime));
            currentFadeTime += Time.deltaTime / damageFlashDuration;
            yield return null;
        }

        currentFadeTime = 0;
        yield return new WaitForSeconds(0.1f);

        while (currentFadeTime < 1.0f)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, Mathf.SmoothStep(0.0f, 1.0f, currentFadeTime));
            currentFadeTime += Time.deltaTime / damageFlashDuration;
            yield return null;
        }
    }

    public virtual void SetVelocity(float velocity)
    {
        float targetVelocity = velocity * facingDirection;
        this.velocity.x = Mathf.SmoothDamp(this.velocity.x, targetVelocity, ref velocityXSmoothing, accelerationTimeGrounded);
        this.velocity.y = entityRigidbody.velocity.y;
        entityRigidbody.velocity = this.velocity;
    }


    public void SetVelocityY(float velocityY)
    {
        velocity.Set(velocity.x, velocityY);
        entityRigidbody.velocity = velocity;
    }

    public void SetVelocityX(float velocityX)
    {
        velocity.Set(velocityX, velocity.y);
        entityRigidbody.velocity = velocity;
    }

    public virtual bool CheckWallFront()
    {
        return Physics2D.Raycast(wallCheck.position, transform.right, entityData.wallCheckDistance, entityData.groundLayerMask);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.groundLayerMask);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.groundLayerMask);
    }

    public virtual bool CheckPlayerInAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, facingDirection * transform.right, entityData.aggroRange, entityData.playerLayerMask);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(-facingDirection, 1f, 1f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + (facingDirection * transform.right * entityData.aggroRange));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, entityData.groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * -facingDirection * entityData.wallCheckDistance));
    
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
     }
}
