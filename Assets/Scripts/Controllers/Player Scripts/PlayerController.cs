using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Editor Variables
    //  Ints
    [SerializeField] private int maxStoneQuant;
    [SerializeField] private int playerLifes;
    [SerializeField] private float throwForce;
    //  GameObjects
    [SerializeField] private GameObject grabber;
    [SerializeField] private GameObject stoneIndicator;
    //  Transforms
    [SerializeField] private Transform throwPoint;
    //  Prefabs
    [SerializeField] private GameObject stonePrefab;
    // Player private variables
    //  Ints
    private int stoneCounter = 0;
    // Events
    public GrabberController onTriggerEnterEvent;

    /// <summary>
    /// Get rigidbody and collider size of player
    /// </summary>
    void Start()
    {
        playerLifes = GameStateSingleton.Instance.getCurrentLives();
    }

    /// <summary>
    /// Check if player is on ground and move
    /// </summary>
    void Update()
    {
        PlayerGrabOrShoot();
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
    /// Player Interaction, check if player has a stone and shoot it if he has, else
    /// set grabber active and inactive depending on press and release of the E button.
    /// </summary>
    void PlayerGrabOrShoot()
    {
        if (Input.GetKeyDown(KeyCode.E) && stoneCounter > 0)
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
        else if (Input.GetKeyUp(KeyCode.E))
        {
            grabber.SetActive(false);
        }

    }


    public void takeAnyDamage()
    {
        playerLifes--;
        GameStateSingleton.Instance.setCurrentLifes(playerLifes);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
