using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float homingStrength = 10f;

    [SerializeField] private float projectileDuration = 5f;

    private GameObject PlayerCharacter;
    private GameObject grandma;
    [SerializeField] private Rigidbody2D projectile;

    private Vector3 direction;
    private bool feeding = false;


    private void Awake()
    {
        StartCoroutine(Kill());
        PlayerCharacter = GameObject.FindGameObjectWithTag("Player");

        direction = (PlayerCharacter.transform.position - transform.position);

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private void FixedUpdate()
    {
        if (!feeding)
        {
            projectile.velocity = transform.up * projectileSpeed;


            direction = (PlayerCharacter.transform.position - transform.position).normalized;

            float rot = Vector3.Cross(transform.up, direction).z;
            projectile.angularVelocity = rot * homingStrength * 10f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        

        if (collision.gameObject.CompareTag("Player") && player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (feeding)
        {
            EnemyGrandma granny = collision.GetComponent<EnemyGrandma>();
            if (collision.gameObject.CompareTag("Grandma") && granny != null)
            {
                granny.Feed();
                Destroy(gameObject);
            }
        }
    }

    public void FeedGrandma()
    {
        Debug.Log("FUCO");
        feeding = true;

        grandma = GameObject.FindGameObjectWithTag("Grandma");

        Vector3 direction = grandma.transform.position - transform.position;
        projectile.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(projectileDuration);
        Destroy(gameObject);
    }
}
