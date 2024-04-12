using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Void : MonoBehaviour
{
    //component

    [Header("Components")]

    [SerializeField] GameObject playerCharacter;
    public PlayerHealth playerHealth;
    [SerializeField] Transform[] voidBubbles;
    [SerializeField] Light2D[] voidLights;


    [Header("Variables")]

    //damage variables
    [SerializeField] private int damage = 1;

    [SerializeField] private float damageInterval = 1f;
    private float timer = -1f;

    [Header("Bubbles")]


    //bubble variables
    [SerializeField] private float voidBubbleSize = 3f;
    private float[] bubbleSizes;
    [SerializeField] private bool isProtected = false;
    private bool drawGizmoz = false;
    [SerializeField] private int arrayCounter = 0;


    [Header("Void Bar")]
    [SerializeField] private Slider voidSlider;
    [SerializeField] private float voidBreath = 0f;
    [SerializeField] private float voidBreathMax = 30f;
    [SerializeField] private float voidEffect = 1f;
    private bool voidDamaged = false;


    private void Awake()
    {
        drawGizmoz = true;
        bubbleSizes = new float[voidBubbles.Length];

        for (int i = 0; i < voidBubbles.Length; i++)
        {
            bubbleSizes[i] = Random.Range(1, voidBubbleSize);
            voidLights[i].pointLightOuterRadius = bubbleSizes[i];
        }

        voidBreath = voidBreathMax;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && voidDamaged && playerHealth != null)
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

        VoidBreath();
    }

    private void VoidBreath()
    {
        if (isProtected && voidBreath <= voidBreathMax)
        {
            voidBreath += voidEffect * Time.deltaTime;
        }
        else if (voidBreath >= 0)
        {
            voidBreath -= voidEffect * Time.deltaTime;
        }

        if (voidBreath < 1)
        {
            voidDamaged = true;
        }
        else
        {
            voidDamaged = false;
        }

        float fillValue = voidBreath / voidBreathMax;
        voidSlider.value = fillValue;
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

