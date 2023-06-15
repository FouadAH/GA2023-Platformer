using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    Collisions collisions;
    Vector2 moveInputDirection;
    Vector2 targetVelocity;

    public float movementSpeed = 8f;
    public float gravity = -1f;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        collisions = GetComponent<Collisions>();
    }

    private void FixedUpdate()
    {
        CalculateVelocity_Simple();

        Walk();
        CalculateGravity();
    }

    public void CalculateVelocity_Simple()
    {
        playerRigidbody.velocity = targetVelocity;
    }

    public void Walk()
    {
        targetVelocity.x = moveInputDirection.x * movementSpeed;
    }

    public void CalculateGravity()
    {
        if (!collisions.onGround)
        {
            targetVelocity.y += gravity;
        }
        else
        {
            targetVelocity.y = Mathf.Clamp(targetVelocity.y, 0, Mathf.Infinity);
        }
    }

    //===================Inputs=======================//

    public void OnMove(InputValue inputValue)
    {
        moveInputDirection = inputValue.Get<Vector2>().normalized;
    }
}
