using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    //stats values
    public float hitPoints = 0;
    public float hitPointMaximum = 50;
    [SerializeField] private float iFrameLength = .3f;
    
    [SerializeField] private bool immune = false;

    //unity module acess
    public Movement movement;
    [SerializeField] private SpriteRenderer playerSprite;

    //private values
    private float iFrames;
    private bool isFlashing = false;
    private bool isHurting = false;
    private bool isHealing = false;



    void Awake()
    {
        hitPoints = hitPointMaximum;
    }

    void FixedUpdate()
    {
        if (iFrames >= 0)
        {
            immune = true;
            iFrames -= Time.deltaTime;
        }
        else 
        {
            immune = false;
        }

        if (immune && !isFlashing)
        {
            StartCoroutine(ImmunityFlashing());
        }
    }

    public void TakeDamage(int damage)
    {
        if (!immune && !movement.isDodging)
        {
            hitPoints -= damage;
            iFrames = iFrameLength;
            if (!isHurting)
            {
                StartCoroutine(DamageFlashing());
            }
        }

        if (hitPoints <= 0)
        {
            //temporary in place of what actually happens when the player dies
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void TakeDamageIgnoreIFrames(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            //temporary in place of what actually happens when the player dies
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void RegainHealth(int healing)
    {
        hitPoints += healing;

        if (!isHealing)
        {
            StartCoroutine(HealFlashing());
        }
    }

    private IEnumerator ImmunityFlashing()
    {
        isFlashing = true;

        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, .5f);
        yield return new WaitForSeconds(.2f);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);

        isFlashing = false;
    }

    private IEnumerator DamageFlashing()
    {
        isHurting = true;
        for (int i = 0; i < 2; i++)
        {
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(.4f);
            playerSprite.color = new Color(0, playerSprite.color.g, playerSprite.color.b, playerSprite.color.a);
        }
        isHurting = false;
    }

    private IEnumerator HealFlashing()
    {
        isHealing = true;
        for (int i = 0; i < 2; i++)
        {
            playerSprite.color = Color.green;
            yield return new WaitForSeconds(.4f);
            playerSprite.color = new Color(playerSprite.color.r, 0, playerSprite.color.b, playerSprite.color.a);
        }
        isHealing = false;
    }
}

