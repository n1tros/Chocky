using System.Collections;
using System.Collections.Generic;
 using UnityEngine;

public class EdgeCheck : MonoBehaviour
{
    public bool EdgeHit { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ledge Hang"))
        {
            EdgeHit = true;   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ledge Hang"))
        {
            EdgeHit = false;
        }
    }
}
