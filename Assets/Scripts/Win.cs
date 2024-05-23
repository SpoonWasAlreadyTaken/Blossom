using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    void Awake()
    {
        StartCoroutine(DelayedVictory());
    }

    IEnumerator DelayedVictory()
    {
        yield return new WaitForSeconds(2f);
        health.Win();
    }
}
