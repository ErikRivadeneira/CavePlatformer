using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Editor Variables 
    //  Floats
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float zCamOffset;
    [SerializeField] private float yCamOffset;
    //  Transforms
    [SerializeField] private Transform groundPoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform wallCheck;
    //  Misc
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask wallLayer;

    // Private Variables
    //  Bools
    private bool isOnGround = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool doubleJump;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    //  Floats
    private float horizontal;
    private float wallSlidingSpeed = 2f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    //  Misc
    private Rigidbody2D rb;
    private Vector2 colliderSize;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        colliderSize = this.GetComponent<CapsuleCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        //Player Ground Check
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, groundLayer);
        if (!GameStateSingleton.Instance.getIsGameOver() || !GameStateSingleton.Instance.getIsGamePaused())
        {
            MovePlayer();
        }
    }

    public void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

    }

    /// <summary>
    /// Player movement, crouching logic, if player presses S key, reduce collider size on Y axis to
    /// half, else return collider to its original size.
    /// </summary>
    void Crouch()
    {
        isCrouching = Input.GetKey(KeyCode.S) ? true : false;

        if (isCrouching)
        {
            this.GetComponent<CapsuleCollider2D>().size = new Vector2(colliderSize.x, colliderSize.y / 2);
        }
        else
        {
            this.GetComponent<CapsuleCollider2D>().size = colliderSize;
        }
    }
    
    /// <summary>
    /// Player movement, have player flip depending on bool and horizontal input axis
    /// </summary>
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// Checks if player is over a wall or not
    /// </summary>
    /// <returns>True if player is over wall, False if not</returns>
    bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    /// <summary>
    /// Player movement, checks if player is on ground and/or has double jump and lets player 
    /// jump acocordingly.
    /// </summary>
    void Jump()
    {
        // if player is on ground and doesn't press jump button set double jump to false
        if (isOnGround && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        // if player presses jump and is on ground or has doublejump set velocity on y and change double jump to opposite value
        if (Input.GetButtonDown("Jump"))
        {
            if (isOnGround || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJump = !doubleJump;
            }
        }
        // If add 0.05f of force to player when releasing jump key, lets the player jump more or less depending on how much jump is pressed
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.05f);
        }
    }

    /// <summary>
    /// Set horizontal value and flip based on input
    /// </summary>
    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //Flip character when moving, player can't flip while wall jumping 
        if (!isWallJumping)
        {
            Flip();
        }
    }

    /// <summary>
    /// Function that controlls All player movement and camera follow.
    /// </summary>
    void MovePlayer()
    {
        // Player Jump
        Jump();
        // Horizontal movement
        Move();
        // Player Crouch
        Crouch();
        // Camera follow
        cameraTransform.position = new Vector3(this.transform.position.x, this.transform.position.y + yCamOffset, this.transform.position.z + zCamOffset);
        // Player WallSlide
        WallSlide();
        // Player WallJump
        WallJump();
    }

    /// <summary>
    /// Set wall jumping bool to false
    /// </summary>
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    /// <summary>
    /// Player movement, lets player wall jump when player is sliding on wall
    /// </summary>
    private void WallJump()
    {
        // if player is wall sliding set jumping direction, reset walljump counter and set wall jumping bool to false. 
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
        }
        // if player isnt wall jumping reduce counter based on delta time
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        // If player presses jump then make player character jump, flip direction and set walljump counter to 0
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            // flip player if player is not looking in the same direction of the jump
            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            // make player stop wall jumping if player is on air too long
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    /// <summary>
    /// Player movement, lets player wall slide when player is on a wall and cancels stop wall jumping invoke
    /// </summary>
    private void WallSlide()
    {
        if (isWalled() && !isOnGround && horizontal != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            isWallSliding = false;
        }
    }
}
