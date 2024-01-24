using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RatEnemy : MonoBehaviour
{
    [SerializeField] private DetectionController onTriggerEnter2DEvent;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;
    [SerializeField] private Animator ratAnim;
    [SerializeField] private AudioSource ratSource;
    [SerializeField] private List<AudioClip> ratSounds;

    private int chargeDirection = 1;
    private bool facingRight = true;
    private bool isCharging = false;
    private bool ratAlive = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            float savedSfxVolume = PlayerPrefs.GetFloat("SFXVol");
            ratSource.volume = savedSfxVolume;
        }
    }

    private void Update()
    {
        if(isCharging && ratAlive && !GameStateSingleton.Instance.getIsGameOver())
        {
            Charging();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            // get enemy collider and player position in x when collision happens
            Collider2D ratCollider = this.gameObject.GetComponent<Collider2D>();
            float playerX = collision.gameObject.transform.position.x;
            // if player position is within X axis bounds of enemy collider, enemy dies, else, enemy takes damage
            if (playerX < ratCollider.bounds.max.x && playerX > ratCollider.bounds.min.x) 
            {
                collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x, 8f);
                Die();
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().takeAnyDamage();
            }
        }
    }

    /// <summary>
    /// Add listener for grabber trigger on object enabled
    /// </summary>
    private void OnEnable()
    {
        onTriggerEnter2DEvent.onTriggerEnter2D.AddListener(PrepareCharge);
    }

    /// <summary>
    /// Add Listener for grabber trigger on object disabled
    /// </summary>
    private void OnDisable()
    {
        onTriggerEnter2DEvent.onTriggerEnter2D.AddListener(PrepareCharge);
    }

    private void Die()
    {
        rb.velocity = Vector2.zero;
        ratAnim.SetBool("IsAlive", false);
        ratAlive = false;
        ratSource.PlayOneShot(ratSounds[0]);
        Destroy(this.gameObject, 1f);
    }



    private void PrepareCharge(Collider2D collision)
    {
        
        if (collision.gameObject.tag.Equals("Player") && ratAlive)
        {
            // add animation for prepare charge
            ratAnim.SetBool("Charging", true);
            // get x from positions of player and rat;
            float currentX = this.transform.position.x;
            float playerX = collision.gameObject.transform.position.x;
            // flip direction of charge if necesary.
            Flip(currentX, playerX);
            Invoke(nameof(BeginCharge), 0.6f);
            Invoke(nameof(StopCharge), chargeDuration);
        }
    }

    private void Flip(float currentPos, float playerPos)
    {
        if(facingRight && playerPos < currentPos || !facingRight && playerPos > currentPos)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            chargeDirection *= -1;
            transform.localScale = localScale;
        }
    }

    private void Charging()
    {
        rb.velocity = new Vector2(chargeSpeed * chargeDirection, rb.velocity.y);
    }

    private void BeginCharge()
    {
        // Start charging animation
        isCharging = true;
    }

    private void StopCharge()
    {
        // set animation back to idle
        isCharging = false;
        ratAnim.SetBool("Charging", false);
    }
}
