using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [SerializeField] private int hitPoints;
    [SerializeField] private int maximumHitPoints = 5;

    [SerializeField] private SpriteRenderer enemySprite;

    [Header("Dummy Settings")]

    [SerializeField] private bool isDummy = false;
    [SerializeField] private TextMeshProUGUI hitPointDisplay;



    private void Awake()
    {
        hitPoints = maximumHitPoints;
    }

    public void EnemyTakeDamage(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0 && !isDummy)
        {
            Destroy(this.gameObject);
        }

        if (isDummy && hitPointDisplay != null)
        {
            hitPointDisplay.text = "Dummy: " + hitPoints + "/" + maximumHitPoints;
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
