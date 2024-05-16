using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();


        if (collision.gameObject.CompareTag("Player") && player != null)
        {
            player.TakeDamage(10000);
        }

    }
}
