using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollectable : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.GetComponent<PlayerHealth>().noteCount++;
        }
    }
}
