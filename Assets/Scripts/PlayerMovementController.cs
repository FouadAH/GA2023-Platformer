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
    public float jumpForce = 2f;
    public float gravity = -1f;

    float facingDirection = -1;

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

        //Check if we colliding with a wall while moving in that direction 
        bool runningIntoLeftWall = collisions.onLeftWall && targetVelocity.x < 0;
        bool runningIntoRightWall = collisions.onRightWall && targetVelocity.x > 0;

        //Set the target X velocity to 0 to avoid getting stuck on the wall 
        if (runningIntoLeftWall || runningIntoRightWall)
        {
            targetVelocity.x = 0;
        }

        Flip();
    }

    public void CalculateGravity()
    {
        if (!collisions.onGround)
        {
            targetVelocity.y += gravity;
        }
        else
        {
            //When on the ground, clamp the target Y velovity to 0 to avoid getting stuck in the floor
            targetVelocity.y = Mathf.Clamp(targetVelocity.y, 0, Mathf.Infinity); 
        }
    }

    public void Flip()
    {
        if (moveInputDirection.x != 0)
        {
            if (facingDirection != moveInputDirection.x)
            {
                facingDirection *= -1;
                transform.localScale = new Vector3(-facingDirection, 1);
            }
        }
    }

    //===================Inputs=======================//

    public void OnMove(InputValue inputValue)
    {
        moveInputDirection = inputValue.Get<Vector2>().normalized;
    }

    public void OnJump()
    {
        if (collisions.onGround)
        {
            targetVelocity.y = jumpForce;
        }
    }
}
