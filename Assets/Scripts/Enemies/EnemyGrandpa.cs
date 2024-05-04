using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class EnemyGrandpa : MonoBehaviour
{
    [Header("Attack")]

    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float attackTime = 1f;



    [Header("Movement")]

    [SerializeField] private float speed;
    [SerializeField] private float tooClose;
    [SerializeField] private int exhaustionMax = 5;
    [SerializeField] private float exhaustionTimer = 4f;
    

    [Header("Inputs")]

    private GameObject player;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Animator animGrandpa;
    [SerializeField] private AudioSource audioWalk;
    [SerializeField] private AudioSource audioThrow;
    [SerializeField] private Rigidbody2D grandpa;


    //Private inaccessable 
    private float attackCD;
    private float distance;
    private bool isAttacking;
    private float horizontal;
    private int playerDirection;
    private bool above;
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

        if (exhaustion < exhaustionMax)
        {
            Attack();


            if (distance > tooClose)
            {
                Movement();
                audioWalk.enabled = true;
            }
            else
            {
                audioWalk.enabled = false;
            }
        }
        else if (!isExhausted)
        {
            StartCoroutine(ExhaustionReset());
        }

        if (distance < tooClose)
        {
            horizontal = 0;
        }



        animGrandpa.SetFloat("GrandpaSpeed", Mathf.Abs(horizontal));

    }

    private void Movement()
    {
        if (!above)
        {
            horizontal = playerDirection;
        }
        else
        {
            horizontal = 0;
        }

        grandpa.velocity = new Vector2(horizontal * speed, grandpa.velocity.y);

    }

    private void Attack()
    {

        if (distance < attackRange && !isAttacking)
        {
            attackCD -= Time.deltaTime;
        }

        if (attackCD <= 0 && distance <= attackRange)
        {
            StartCoroutine(AttackClub());
        }
    }

    private void DirectionFinder()
    {
        Vector2 direction = player.transform.position - transform.position;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        if (angle > -45 && angle < 45)
        {
            above = true;
        }
        else
        {
            above = false;
        }


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
        if (faceR && playerDirection > 0 || !faceR && playerDirection < 0)
        {
            faceR = !faceR;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator AttackClub()
    {
        attackCD = attackSpeed;
        isAttacking = true;
        animGrandpa.SetTrigger("GrandpaAttack");
        Debug.Log("bonk");


        yield return new WaitForSeconds(attackTime);

        audioThrow.Play();

        Instantiate(projectile, attackOrigin.position, attackOrigin.rotation);
        isAttacking = false;
        exhaustion++;
    }

    private IEnumerator ExhaustionReset()
    {
        isExhausted = true;
        animGrandpa.SetBool("IsTired", true);
        Debug.Log("Exhausted");


        yield return new WaitForSeconds(exhaustionTimer);
        exhaustion = 0;
        isExhausted = false;
        animGrandpa.SetBool("IsTired", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, tooClose);
    }
}
