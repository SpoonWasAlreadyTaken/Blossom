using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float timeToFall = 3f;
    [SerializeField] private float wobble = 5f;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private float transparancy;
    private bool activated = false;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();    

        rigidBody.isKinematic = true;

        transparancy = 1f;
        activated = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated)
        {
            StartCoroutine(StartFalling());

        }
    }

    IEnumerator StartFalling()
    {
        activated = true;

        for (int i = 0; i < 30; i++) 
        {
            transform.Rotate(0,0, wobble, Space.Self);
            wobble = wobble * -1.025f;

            yield return new WaitForSeconds(timeToFall / 30);
            transform.Rotate(0, 0, wobble, Space.Self);
        }

        StartCoroutine(Destroy());
        rigidBody.isKinematic = false;
    }

    IEnumerator Destroy()
    {
        for (int i = 0; i < 30; i++)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.r, sprite.color.r, transparancy);
            transparancy -= 0.04f;

            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
}
