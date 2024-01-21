using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Destructible"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            // Insert enemy damage logic
        }
        
    }
}
