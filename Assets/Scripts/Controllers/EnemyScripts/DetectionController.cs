using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionController : MonoBehaviour
{
    public UnityEvent<Collider2D> onTriggerEnter2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(onTriggerEnter2D != null) this.onTriggerEnter2D.Invoke(collision);
    }
}
