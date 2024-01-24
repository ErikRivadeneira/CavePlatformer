using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallHazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().takeAnyDamage();
        }
    }
}
