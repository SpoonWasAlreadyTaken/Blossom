using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    public PlayerHealth playerHealth;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
