using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStick : MonoBehaviour
{
    [Header("Attack Settings")]

    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 1f;


    [Header("Unity Inputs")]

    [SerializeField] private Animator animPlayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask damagables;

    //hiden values

    private float timer1;
    private bool isAttacking = false;


    void Update()
    {
        if (!isAttacking)
        {
            timer1 -= Time.deltaTime;
        }

        if (Input.GetKeyDown("f") && timer1 <= 0 && !isAttacking)
        {
            StartCoroutine(AttackStick());
        }

    }

    private IEnumerator AttackStick()
    {
        isAttacking = true;
        animPlayer.SetTrigger("AttackStick");


        yield return new WaitForSeconds(attackSpeed);

        Collider2D[] damagable = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damagables);
        foreach (Collider2D enemy in damagable)
        {
            enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(attackDamage);
        }

        timer1 = attackCooldown;
        isAttacking = false;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}