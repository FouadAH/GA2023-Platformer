using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Header("Collision Settings")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset;

    [Header("Collision Flags")]
    public bool onGround;

    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
    }
}
