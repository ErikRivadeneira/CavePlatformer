using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Editor Variables
    //  Ints
    [SerializeField] private int maxStoneQuant;
    [SerializeField] private int playeLifes;
    //  Floats
    [SerializeField] private float speed;
    [SerializeField] private float throwForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float zCamOffset;
    [SerializeField] private float yCamOffset;
    //  GameObjects
    [SerializeField] private GameObject grabber;
    [SerializeField] private GameObject stoneIndicator;
    //  Transforms
    [SerializeField] private Transform groundPoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Transform wallCheck;
    //  UI
    //  Misc
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    //  Prefabs
    [SerializeField] private GameObject stonePrefab;

    // Player private variables
    //  Bools
    private bool isOnGround = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool doubleJump;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    //  Ints
    private int stoneCounter = 0;
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


    // Events
    public GrabberController onTriggerEnterEvent;

    /// <summary>
    /// Get rigidbody and collider size of player
    /// </summary>
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        colliderSize = this.GetComponent<CapsuleCollider2D>().size;
    }

    /// <summary>
    /// Check if player is on ground and move
    /// </summary>
    void Update()
    {
        //Player Ground Check
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, groundLayer);

        PlayerMovement();
    }

    public void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
    /// Set horizontal value and flip based on input
    /// </summary>
    void MovePlayer()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //Flip character when moving, player can't flip while wall jumping 
        if (!isWallJumping)
        {
            Flip();
        }
    }

    /// <summary>
    /// Add listener for grabber trigger on object enabled
    /// </summary>
    private void OnEnable()
    {
        onTriggerEnterEvent.onTriggerEnter2D.AddListener(OnGrabberEnter);
    }

    /// <summary>
    /// Add Listener for grabber trigger on object disabled
    /// </summary>
    private void OnDisable()
    {
        onTriggerEnterEvent.onTriggerEnter2D.AddListener(OnGrabberEnter);
    }

    /// <summary>
    /// Determine if object is inside grabber, if the object is a stone pick it up and destroy it
    /// from the level.
    /// </summary>
    /// <param name="collision"></param>
    private void OnGrabberEnter(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Stone") && stoneCounter < maxStoneQuant) 
        {
            stoneCounter++;
            stoneIndicator.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Player movement, crouching logic, if player presses S key, reduce collider size on Y axis to
    /// half, else return collider to its original size.
    /// </summary>
    void PlayerCrouch()
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
    /// Player Interaction, check if player has a stone and shoot it if he has, else
    /// set grabber active and inactive depending on press and release of the E button.
    /// </summary>
    void PlayerGrabOrShoot()
    {
        if(Input.GetKeyDown(KeyCode.E) && stoneCounter > 0) 
        {
            stoneCounter--;
            GameObject thrownStone = Instantiate(stonePrefab, throwPoint.position, throwPoint.rotation);
            thrownStone.GetComponent<Rigidbody2D>().AddForce(transform.right * throwForce, ForceMode2D.Impulse);
            stoneIndicator.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            grabber.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.E)) 
        {
            grabber.SetActive(false);
        }

    }

    /// <summary>
    /// Function that controlls All player movement and camera follow.
    /// </summary>
    void PlayerMovement()
    {
        // Player Jump
        PlayerJump();
        // Horizontal movement
        MovePlayer();
        // Player Crouch
        PlayerCrouch();
        // Player Grab or Shoot
        PlayerGrabOrShoot();
        // Camera follow
        cameraTransform.position = new Vector3(this.transform.position.x,this.transform.position.y + yCamOffset, this.transform.position.z + zCamOffset);
        // Player WallSlide
        WallSlide();
        // Player WallJump
        WallJump();
    }

    /// <summary>
    /// Player movement, checks if player is on ground and/or has double jump and lets player 
    /// jump acocordingly.
    /// </summary>
    void PlayerJump()
    {
        // if player is on ground and doesn't press jump button set double jump to false
        if(isOnGround && !Input.GetButton("Jump"))
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
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.05f);
        }
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
        if (Input.GetButtonDown("Jump") && wallJumpingCounter>0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            // flip player if player is not looking in the same direction of the jump
            if(transform.localScale.x != wallJumpingDirection)
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
        if(isWalled() && !isOnGround && horizontal != 0)
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
