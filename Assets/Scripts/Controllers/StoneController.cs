using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> stoneSounds = new List<AudioClip>();
    private AudioSource stoneSource;

    private void Start()
    {
        stoneSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Destructible"))
        {
            Destroy(collision.gameObject);
            stoneSource.PlayOneShot(stoneSounds[0]);
            Destroy(this.gameObject,0.5f);
        }
    }
}
