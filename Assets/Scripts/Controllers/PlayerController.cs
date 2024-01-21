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
    [SerializeField] private Transform groundPoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject grabber;
    [SerializeField] private GameObject stoneIndicator;
    //  UI
    //  Misc
    [SerializeField] private LayerMask groundLayer;
    //  Prefabs
    [SerializeField] private GameObject stonePrefab;

    // Player private variables
    private Rigidbody2D rb;
    private bool isOnGround = true;
    private bool doubleJump;
    private bool isCrouching = false;
    private Vector2 colliderSize;
    private int stoneCounter = 0;

    // Events
    public GrabberController onTriggerEnterEvent;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        colliderSize = this.GetComponent<CapsuleCollider2D>().size;
    }

    void Update()
    {
        //Player Ground Check
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, groundLayer);

        PlayerMovement();
    }

    void MovePlayer()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        //Flip character when moving
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 2f, 1f);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1f, 2f, 1f);
        }
    }

    private void OnEnable()
    {
        onTriggerEnterEvent.onTriggerEnter2D.AddListener(OnGrabberEnter);
    }

    private void OnDisable()
    {
        onTriggerEnterEvent.onTriggerEnter2D.AddListener(OnGrabberEnter);
    }

    private void OnGrabberEnter(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Stone") && stoneCounter < maxStoneQuant) 
        {
            stoneCounter++;
            stoneIndicator.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

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

    void PlayerMovement()
    {
        // Horiz movement
        MovePlayer();

        // Player Crouch
        PlayerCrouch();

        // Player Grab or Shoot
        PlayerGrabOrShoot();

        // Camera follow
        cameraTransform.position = new Vector3(this.transform.position.x,this.transform.position.y + yCamOffset, this.transform.position.z + zCamOffset);

        // Player Jump
        PlayerJump();
    }

    void PlayerJump()
    {
        if(isOnGround && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isOnGround || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                doubleJump = !doubleJump;
            }
        }
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.05f);
        }
    }
}
