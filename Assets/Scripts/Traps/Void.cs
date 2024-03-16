using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Void : MonoBehaviour
{
    //component
    [SerializeField] GameObject playerCharacter;
    public PlayerHealth playerHealth;
    [SerializeField] Transform[] voidBubbles;
    [SerializeField] Light2D[] voidLights;

    //damage variables
    [SerializeField] private int damage = 1;

    [SerializeField] private float damageInterval = 1f;
    private float timer = -1f;

    //bubble variables
    [SerializeField] private float voidBubbleSize = 3f;
    private float[] bubbleSizes;
    [SerializeField] private bool isProtected = false;
    private bool drawGizmoz = false;
    [SerializeField] private int arrayCounter = 0;


    private void Start()
    {
        drawGizmoz = true;
        bubbleSizes = new float[voidBubbles.Length];

        for (int i = 0; i < voidBubbles.Length; i++)
        {
            bubbleSizes[i] = Random.Range(1, voidBubbleSize);
            voidLights[i].pointLightOuterRadius = bubbleSizes[i];
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && !isProtected && playerHealth != null)
        {
            playerHealth.TakeDamageIgnoreIFrames(damage);
            timer = damageInterval;
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < voidBubbles.Length; i++)
        {
            float distance = Vector2.Distance(voidBubbles[i].position, playerCharacter.transform.position);

            if (distance < bubbleSizes[i])
            {
                isProtected = true;
            }
            else
            {
                arrayCounter++;
            }
        }
        if (arrayCounter == voidBubbles.Length)
        {
            isProtected = false;
            arrayCounter = 0;
        }
        arrayCounter = 0;
    }


    private void OnDrawGizmos()
    {
        if (drawGizmoz)
        {
            for (int i = 0; i < voidBubbles.Length; i++)
            {
                Gizmos.DrawWireSphere(voidBubbles[i].position, bubbleSizes[i]);
            }
        }
    }
}
