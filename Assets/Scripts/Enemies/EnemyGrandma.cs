using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrandma : MonoBehaviour
{
    [Header("Attack")]

    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float attackTime = 1f;



    [Header("Movement")]

    [SerializeField] private float speed;

    [SerializeField] private float followDistance = 10f;
    [SerializeField] private int exhaustionMax = 5;
    [SerializeField] private float exhaustionTimer = 4f;


    [Header("Inputs")]

    private GameObject player;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Animator animGrandma;
    [SerializeField] private AudioSource audioThrow;
    [SerializeField] private Rigidbody2D grandma;
    [SerializeField] private SpriteRenderer spriteGrandma;


    //Private inaccessable 
    private float attackCD;
    private float distance;
    private bool isAttacking;
    private float horizontal;
    private int playerDirection;
    private bool faceR;
    private int exhaustion;
    private bool isExhausted = false;


    void Awake()
    {
        attackCD = attackSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        exhaustion = 0;
    }


    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);


        Flip();
        DirectionFinder();

        if (exhaustion < exhaustionMax && followDistance > distance)
        {
            Attack();

            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed);

        }
        else if (!isExhausted)
        {
            StartCoroutine(ExhaustionReset());
        }
    }


    private void Attack()
    {

        if (distance < attackRange && !isAttacking)
        {
            attackCD -= Time.deltaTime;
        }

        if (attackCD <= 0 && distance <= attackRange)
        {
            StartCoroutine(AttackCupcake());
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

    private IEnumerator AttackCupcake()
    {
        attackCD = attackSpeed;
        isAttacking = true;
        animGrandma.SetTrigger("Attack");
        Debug.Log("Food");


        yield return new WaitForSeconds(attackTime);

        audioThrow.Play();

        Instantiate(projectile, attackOrigin.position, attackOrigin.rotation);
        isAttacking = false;
    }

    public void Feed()
    {
        exhaustion++;

        StartCoroutine(FeedingFlash());
    }

    private IEnumerator FeedingFlash()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteGrandma.color = Color.red;
            yield return new WaitForSeconds(.1f);
            spriteGrandma.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator ExhaustionReset()
    {
        isExhausted = true;
        animGrandma.SetBool("Tired", true);
        Debug.Log("Exhausted");


        yield return new WaitForSeconds(exhaustionTimer);
        exhaustion = 0;
        isExhausted = false;
        animGrandma.SetBool("Tired", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, followDistance);
    }
}
