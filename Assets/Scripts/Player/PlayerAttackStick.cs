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
    [SerializeField] private bool comboAttacks = false;
    [SerializeField] private float comboTime = 1f;
    [SerializeField] private int comboMaxStacks = 4;


    [Header("Unity Inputs")]

    [SerializeField] private Animator animPlayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask damagables;
    [SerializeField] private AudioSource audioAttack;

    //hiden values

    private float timer1;
    private bool isAttacking = false;
    private int comboStacks = 0;
    private float comboResetTimer;


    void Update()
    {
        if (!isAttacking)
        {
            timer1 -= Time.deltaTime;
        }

        if (Input.GetKeyDown("f") && timer1 <= 0 && !isAttacking)
        {
            if (comboAttacks && comboStacks > 0) 
            {
                StartCoroutine(AttackStickCombo());
            }
            else
            {
                StartCoroutine(AttackStick());
            }
        }

        if (comboStacks > 0)
        {
            comboResetTimer -= Time.deltaTime;
            
            if (comboResetTimer < 0)
                {
                    comboStacks = 0;
                }
        }
    }

    private IEnumerator AttackStick()
    {
        comboResetTimer = comboTime;

        if (comboStacks == 0)
        {
            animPlayer.SetFloat("WeaponSpeed", 1f);
            audioAttack.pitch = 1f;
        }
        isAttacking = true;
        animPlayer.SetTrigger("AttackStick");


        yield return new WaitForSeconds(attackSpeed);

        audioAttack.Play();

        Collider2D[] damagable = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damagables);
        foreach (Collider2D enemy in damagable)
        {
            enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(attackDamage);
        }

        timer1 = attackCooldown;
        isAttacking = false;

        comboStacks++;

        if (comboAttacks)
        {
            comboStacks++;
        }
    }

    private IEnumerator AttackStickCombo()
    {
        isAttacking = true;
        comboResetTimer = comboTime;

        if (comboStacks < comboMaxStacks)
        {
            animPlayer.SetFloat("WeaponSpeed", 2f);
            animPlayer.SetTrigger("AttackStick");

            audioAttack.pitch = 2f;
            audioAttack.Play();

            yield return new WaitForSeconds(attackSpeed / 2);
        }
        else
        {
            animPlayer.SetFloat("WeaponSpeed", 0.7f);
            animPlayer.SetTrigger("AttackStick");

            audioAttack.pitch = 0.5f;
            audioAttack.Play();

            yield return new WaitForSeconds(attackSpeed / 0.7f);
        }

            Collider2D[] damagable = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damagables);
            foreach (Collider2D enemy in damagable)
            {
                if (comboStacks < comboMaxStacks)
                {
                    enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(attackDamage);
                }
                else
                {
                    enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(attackDamage * 2);
                }
            }

            timer1 = attackCooldown / comboStacks;
            isAttacking = false;
            comboStacks++;
            if (comboStacks > comboMaxStacks)
            {
                comboStacks = 0;
            }   
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}