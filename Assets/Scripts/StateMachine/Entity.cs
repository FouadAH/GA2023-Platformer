using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

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

    private Vector2 velocityWorkspace;
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

    public virtual void LateUpdate()
    {
        stateMachine.currentState.LatePhysicsUpdate();   
    }

    public virtual void SetVelocity(float velocity)
    {
        float targetVelocity = velocity * facingDirection;
        velocityWorkspace.x = Mathf.SmoothDamp(velocityWorkspace.x, targetVelocity, ref velocityXSmoothing, accelerationTimeGrounded);
        velocityWorkspace.y = entityRigidbody.velocity.y;
        entityRigidbody.velocity = velocityWorkspace;
    }


    public void SetVelocityY(float velocityY)
    {
        velocityWorkspace.Set(velocityWorkspace.x, velocityY);
        entityRigidbody.velocity = velocityWorkspace;
    }

    public void SetVelocityX(float velocityX)
    {
        velocityWorkspace.Set(velocityX, velocityWorkspace.y);
        entityRigidbody.velocity = velocityWorkspace;
    }

    public virtual bool CheckWallFront()
    {
        return Physics2D.Raycast(wallCheck.position, transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, -transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckWall()
    {
        return CheckWallFront() || CheckWallBack();
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, facingDirection * transform.right, entityData.aggroRange, entityData.whatIsPlayer);
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

        if (facingDirection != 0)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * -facingDirection * entityData.wallCheckDistance));
        }
        else
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * 1 * entityData.wallCheckDistance));
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * -1 * entityData.wallCheckDistance));
        }

        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
