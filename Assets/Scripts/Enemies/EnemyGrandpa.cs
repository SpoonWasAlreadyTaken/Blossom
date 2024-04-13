using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrandpa : MonoBehaviour
{
    [Header("Attack")]

    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float attackTime = 1f;



    [Header("Movement")]

    [SerializeField] private float speed;

    [Header("Inputs")]

    [SerializeField] private GameObject player;
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Animator animGrandpa;


    //Private inaccessable 
    private float attackCD;
    private bool isAttacking;


    void Awake()
    {
        attackCD = attackSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Debug.Log(attackCD);
        }
    }


    void FixedUpdate()
    {

        Attack();
    }


    private void Attack()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < attackRange && !isAttacking)
        {
           attackCD -= Time.deltaTime;
        }

       // attackCD -= Time.deltaTime;


        if (attackCD <= 0 && distance <= attackRange)
        {
            StartCoroutine(AttackClub());
        }
    }

    private IEnumerator AttackClub()
    {
        attackCD = attackSpeed;
        isAttacking = true;
        animGrandpa.SetTrigger("GrandpaAttack");
        Debug.Log("bonk");


        yield return new WaitForSeconds(attackTime);
        Instantiate(projectile, attackOrigin.position, attackOrigin.rotation);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
