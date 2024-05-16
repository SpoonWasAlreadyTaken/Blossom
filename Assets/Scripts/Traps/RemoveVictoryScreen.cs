using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveVictoryScreen : MonoBehaviour
{
    private int clicks = 0;

    [SerializeField] private GameObject voidPortal;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            clicks++;
        }

        if (clicks >= 13)
        {
            Instantiate(voidPortal, player.transform.position, player.transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
