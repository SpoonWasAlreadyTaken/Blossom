using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTherapist : MonoBehaviour
{
    [Header("Attack")]

    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float attackTime = 1f;



    [Header("Movement")]

    [SerializeField] private float speed;

    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float exhaustionTimer = 4f;

    [SerializeField] private float tooClose = 3f;

    [SerializeField] private float teleportTime = 3f;
    [SerializeField] private float teleportMishap = 5f;


    [Header("Inputs")]

    private GameObject player;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Animator animTherapist;
    [SerializeField] private AudioSource audioThrow;
    [SerializeField] private Rigidbody2D therapist;
    [SerializeField] private SpriteRenderer spriteTherapist;
    private PlayerHealth health;


    //Private inaccessable 
    private float attackCD;
    private float distance;
    private bool isAttacking;
    private int playerDirection;
    private bool faceR;
    private bool isExhausted = false;
    private bool teleporting = false;
    private int pill;


    void Awake()
    {
        attackCD = attackSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<PlayerHealth>();
    }


    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);


        Flip();
        DirectionFinder();

        if (!isExhausted)
        {
            Attack();

            if (distance > tooClose)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed);

            }
        }

        if (distance > followDistance && !teleporting)
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        teleporting = true;

        for (int i = 0; i < 5; i++)
        {
            spriteTherapist.color = Color.blue;
            yield return new WaitForSeconds(teleportTime / 10f);
            spriteTherapist.color = Color.white;
            yield return new WaitForSeconds(teleportTime / 10f);
        }

        transform.position = player.transform.position + new Vector3(Random.Range(-teleportMishap, teleportMishap), Random.Range(-teleportMishap, teleportMishap), 0);

        teleporting = false;
    }

    private void Attack()
    {

        if (distance < attackRange && !isAttacking)
        {
            attackCD -= Time.deltaTime;
        }

        if (attackCD <= 0 && distance <= attackRange)
        {
            StartCoroutine(AttackPill());
        }
    }

    private void DirectionFinder()
    {
        Vector2 direction = player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;


        if (angle < -45)
        {
            playerDirection = -1;
        }
        else if (angle > 45)
        {
            playerDirection = 1;
        }

    }

    private void Flip()
    {
        if (faceR && playerDirection < 0 || !faceR && playerDirection > 0)
        {
            faceR = !faceR;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator AttackPill()
    {
        attackCD = attackSpeed;
        isAttacking = true;
        animTherapist.SetTrigger("Attack");
        Debug.Log("Cure");


        yield return new WaitForSeconds(attackTime);

        audioThrow.Play();

        if (health.hitPoints < health.hitPointMaximum/3)
        {
            Instantiate(projectiles[2], attackOrigin.position, attackOrigin.rotation);
            Debug.Log("Won");
        }
        else
        {
            pill = Random.Range(0, projectiles.Length - 1);
            Instantiate(projectiles[pill], attackOrigin.position, attackOrigin.rotation);
        }

        isAttacking = false;
    }


    public void Poisoned()
    {
        StartCoroutine(Exhaustion());
    }


    private IEnumerator Exhaustion()
    {
        isExhausted = true;
        Debug.Log("Exhausted");

        for (int i = 0; i < 10; i++)
        {
            spriteTherapist.color = new Color(1, 0, 1, 1);
            yield return new WaitForSeconds(exhaustionTimer / 20);
            spriteTherapist.color = Color.white;
            yield return new WaitForSeconds(exhaustionTimer / 20);
        }

        isExhausted = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, followDistance);
        Gizmos.DrawWireSphere(transform.position, tooClose);
    }
}
