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

    public Animator animator;

    [Header("Movement Settings")]
    public float movementSpeed = 8f;
    public float jumpForce = 2f;
    public float minJumpForce = 0.2f;

    public float gravity = -1f;

    [Header("Damping Settings")]
    public bool smoothDamp = false;
    public float groundedSmoothTime = 0.03f;
    public float airbornSmoothTime = 0.1f;
    float targetVelocityXRef;

    float facingDirection = -1;

    [Header("Jump Buffer")]
    public bool jumpBuffer = true;
    int jumpFrameBuffer;
    public int jumpFrameBufferFrames = 10;

    [Header("Cayote Time")]
    public bool cayoteTime = true;
    public float cayoteTimeMax = 0.1f;
    public float currentCayoteTime;

    [Header("Effects")]
    public ParticleSystem walkVFX;
    public AudioSource walkSFX;
    public AudioSource jumpSFX;
    public AudioSource landSFX;

    public float footstepDelay;
    public float footstepInterval;

    [Header("Data")]
    public PlayerRuntimeDataSO runtimeDataSO;
    public PlayerInputMaster playerInputs;

    bool hasJumped;
    bool isWalking;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        collisions = GetComponent<Collisions>();

        playerInputs = new PlayerInputMaster();
        playerInputs.Enable();
        playerInputs.Player.Jump.started += Jump_started;
        playerInputs.Player.Jump.canceled += Jump_canceled;

        jumpFrameBuffer = 100;
    }

    private void Update()
    {
        moveInputDirection = playerInputs.Player.Move.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        collisions.CheckCollisions();

        if (smoothDamp)
        {
            CalculateVelocity_SmoothDamp();
        }
        else
        {
            CalculateVelocity_Simple();
        }

        Walk();
        Flip();

        JumpCheck();

        CalculateGravity();

        if (cayoteTime)
        {
            CayoteTime();
        }

        if (jumpBuffer)
        {
            JumpBuffer();
        }

        //if (collisions.onGround && hasJumped)
        //{
        //    hasJumped = false;
        //}

        runtimeDataSO.playerPosition = transform.position;
    }

    public void JumpCheck()
    {
        //We check if we landed before allowing another jump
        if (hasJumped && collisions.onGround && targetVelocity.y < 0)
        {
            hasJumped = false;
            landSFX.Play();
        }
    }

    void CayoteTime()
    {
        if (!collisions.onGround && !hasJumped)
        {
            currentCayoteTime += Time.deltaTime;
        }
        else
        {
            currentCayoteTime = 0;
        }
    }

    void JumpBuffer()
    {
        if (!collisions.onGround)
        {
            jumpFrameBuffer++;
        }
        else
        {
            if (jumpFrameBuffer < jumpFrameBufferFrames)
            {
                Jump();
                jumpFrameBuffer = 100;
            }
        }
    }

    public void CalculateVelocity_Simple()
    {
        playerRigidbody.velocity = targetVelocity;
    }

    public void CalculateVelocity_SmoothDamp()
    {
        float smoothTime = (collisions.onGround) ? groundedSmoothTime : airbornSmoothTime;
        Vector2 velocity = playerRigidbody.velocity;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref targetVelocityXRef, smoothTime);
        velocity.y = targetVelocity.y;

        playerRigidbody.velocity = velocity;
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

        animator.SetBool("isWalking", targetVelocity.x != 0);
        WalkSFX();
    }

    void WalkSFX()
    {
        if (targetVelocity.x != 0 && collisions.onGround)
        {
            if (!isWalking)
            {
                isWalking = true;
                StartCoroutine(StartWalkSFX());
            }
        }
        else
        {
            isWalking = false;
            StopAllCoroutines();
        }
    }

    IEnumerator StartWalkSFX()
    {
        yield return new WaitForSeconds(footstepDelay);

        while (isWalking)
        {
            Instantiate(walkSFX).Play();
            yield return new WaitForSeconds(footstepInterval);
        }
    }

    public void CalculateGravity()
    { 
        if (collisions.onTopWall)
        {
            targetVelocity.y = 0;
        }
        
        if (!collisions.onGround)
        {
            targetVelocity.y += gravity;
        }
        else
        {
            //When on the ground, clamp the target Y velovity to 0 to avoid getting stuck in the floor
            targetVelocity.y = Mathf.Clamp(targetVelocity.y, 0, Mathf.Infinity); 
        }

        animator.SetBool("isJumping", !collisions.onGround);
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

    private void Jump_canceled(InputAction.CallbackContext obj)
    {
        if (targetVelocity.y > minJumpForce)
        {
            targetVelocity.y = minJumpForce;
        }
    }

    private void Jump_started(InputAction.CallbackContext obj)
    {
        if (collisions.onGround || currentCayoteTime < cayoteTimeMax)
        {
            if (!hasJumped)
            {
                Jump();
                hasJumped = true;
            }
            else
            {
                jumpFrameBuffer = 0;
            }
        }
        else
        {
            jumpFrameBuffer = 0;
        }
    }

    void Jump()
    {
        targetVelocity.y = jumpForce;
        jumpSFX.Play();
    }
}
