using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private PlayerHealth playerHealth;


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerHealth = player.GetComponent<PlayerHealth>(); 
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
