using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [SerializeField] private int hitPoints;
    [SerializeField] private int maximumHitPoints = 5;

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
    }

}
