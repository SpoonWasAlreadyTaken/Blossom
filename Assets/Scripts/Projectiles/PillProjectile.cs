using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillProjectile : MonoBehaviour
{
    [Header("Stats")]

    [SerializeField] private int damage = 1;

    [SerializeField] private float projectileSpeed = 12f;

    [SerializeField] private float projectileDuration = 5f;

    [SerializeField] private Rigidbody2D projectile;

    [Header("Pill Type")]

    [SerializeField] private bool pillDamage = false;
    [SerializeField] private bool pillPoison = false;
    [SerializeField] private float poisonDuration = 6f;
    [SerializeField] private bool pillGold = false;

    private GameObject PlayerCharacter;


    private void Awake()
    {
        StartCoroutine(Kill());
        PlayerCharacter = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = PlayerCharacter.transform.position - transform.position;
        projectile.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, 30, Space.Self);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (collision.gameObject.CompareTag("Player") && player != null)
        {
            if (pillDamage)
            {
                player.TakeDamage(damage);

            }
            if (pillPoison)
            {
                player.Poison(poisonDuration);

            }
            if (pillGold)
            {
                player.VictoryLap();

            }

            Destroy(gameObject);
        }
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(projectileDuration);
        Destroy(gameObject);
    }

}
