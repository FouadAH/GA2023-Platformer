using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Header("Collision Settings")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;
    public Vector2 topOffset;

    [Header("Collision Flags")]
    public bool onGround;
    public bool onWall;
    public bool onTopWall;
    public bool onRightWall;
    public bool onLeftWall;

    void FixedUpdate()
    {
        //onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        //onTopWall = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, collisionRadius, groundLayer);
        //onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        //onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        //onWall = onRightWall || onLeftWall;
    }

    public void CheckCollisions()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onTopWall = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onWall = onRightWall || onLeftWall;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
