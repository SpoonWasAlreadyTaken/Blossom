using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClubProjectile : MonoBehaviour
{
    public PlayerHealth player;
    [SerializeField] private int damage = 1;

    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float rotationSpeed = 45f;

    [SerializeField] private float projectileDuration = 5f;

    [SerializeField] private GameObject PlayerCharacter;
    [SerializeField] private Rigidbody2D projectile;


    private void Awake()
    {
        StartCoroutine(Kill());
        PlayerCharacter = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = PlayerCharacter.transform.position - transform.position;
        projectile.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (collision.gameObject.CompareTag("Player") && player != null)
        {
            Debug.Log("Ouch");
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(projectileDuration);
        Destroy(gameObject);
    }

}
