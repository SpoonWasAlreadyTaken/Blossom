using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    public float hitPoints;
    public float maximumHitPoints = 5;

    [SerializeField] private SpriteRenderer enemySprite;

    [Header("On Death")]

    [SerializeField] private bool spawnOnDeath = false;
    [SerializeField] private GameObject onDeathSpawn;

    [Header("Health Bar")]

    [SerializeField] private Slider healthBar;


    [Header("Dummy Settings")]

    [SerializeField] private bool isDummy = false;


    private void Awake()
    {
        hitPoints = maximumHitPoints;
    }

    public void EnemyTakeDamage(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0 && !isDummy)
        {
            if (spawnOnDeath && onDeathSpawn != null) 
            {
                Instantiate(onDeathSpawn, transform.position, new Quaternion(0,0,0,0));
                Destroy(gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        if (healthBar != null)
        {
            float fillValue = hitPoints / maximumHitPoints;

            healthBar.value = fillValue;
        }


        StartCoroutine(DamageFlashing());
    }


    private IEnumerator DamageFlashing()
    {
        for (int i = 0; i < 2; i++)
        {
            enemySprite.color = new Color(1, 0.5f, 0.5f, 1);
            yield return new WaitForSeconds(.05f);
            enemySprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(.075f);
        }
    }
}
