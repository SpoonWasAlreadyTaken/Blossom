using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollectable : MonoBehaviour
{
    [SerializeField] private GameObject summonNote;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && summonNote != null)
        {
            Destroy(gameObject);
            collision.GetComponent<PlayerHealth>().noteCount++;
        }
    }
}
