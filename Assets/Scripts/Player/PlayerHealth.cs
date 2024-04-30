using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.HighDefinition;

public class PlayerHealth : MonoBehaviour
{
    [Header("Values")]
    //stats values
    public float hitPoints = 0;
    public float hitPointMaximum = 50;
    [SerializeField] private float iFrameLength = .3f;
    
    [SerializeField] private bool immune = false;

    [SerializeField] private int minSat = 70;

    [Header("Unity Inputs")]

    //unity module acess
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer cloudSprite;
    [SerializeField] private Animator animPlayer;
    
    [Header("Disable Components")]
    public Movement movement;
    [SerializeField] private PlayerAttackStick attackStick;

    [Header("Death Screen")]
    [SerializeField] private GameObject deathScreen;


    //private values
    private float iFrames;
    private bool isFlashing = false;
    private bool isHurting = false;
    private bool isHealing = false;
    private bool dead = false;
    private Volume ppv;
    UnityEngine.Rendering.Universal.ColorAdjustments ca;





    void Awake()
    {
        hitPoints = hitPointMaximum;
        GameObject VPPV = GameObject.FindGameObjectWithTag("PPV");
        ppv = VPPV.GetComponent<Volume>();
        ppv.profile.TryGet<UnityEngine.Rendering.Universal.ColorAdjustments>(out ca);
        deathScreen.SetActive(false);
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

        if (immune && !isFlashing && !dead)
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
            if (!isHurting && !dead)
            {
                StartCoroutine(DamageFlashing());
            }
        }

        if (hitPoints <= 0 && !dead)
        {
            //temporary in place of what actually happens when the player dies
            StartCoroutine(IDied());
        }

        if (ppv != null && ca != null)
        {
            ca.saturation.value = (hitPoints/hitPointMaximum * minSat) - minSat;
        }
    }

    public void TakeDamageIgnoreIFrames(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0 && !dead)
        {
            //temporary in place of what actually happens when the player dies
            StartCoroutine(IDied());
        }

        if (ppv != null && ca != null)
        {
            ca.saturation.value = (hitPoints/hitPointMaximum * minSat) - minSat;
        }
    }

    public void RegainHealth(int healing)
    {
        hitPoints += healing;

        if (!isHealing && !dead)
        {
            StartCoroutine(HealFlashing());
        }
        
        if (ppv != null && ca != null)
        {
            ca.saturation.value = (hitPoints/hitPointMaximum * minSat) - minSat;
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
            if (!dead)
            {
                playerSprite.color = Color.red;
                yield return new WaitForSeconds(.4f);
                playerSprite.color = new Color(0, playerSprite.color.g, playerSprite.color.b, playerSprite.color.a);
            }
        }
        isHurting = false;
    }

    private IEnumerator HealFlashing()
    {
        isHealing = true;
        for (int i = 0; i < 2; i++)
        {
            if(!dead)
            {
                playerSprite.color = Color.green;
                yield return new WaitForSeconds(.4f);
                playerSprite.color = new Color(playerSprite.color.r, 0, playerSprite.color.b, playerSprite.color.a);
            }
        }
        isHealing = false;
    }


    IEnumerator IDied()
    {
        dead = true;
        animPlayer.SetTrigger("Died");
        movement.enabled = false;
        attackStick.enabled = false;

        playerSprite.color = new Color(0.5f, 0.5f, 0.5f, playerSprite.color.a);
        StartCoroutine(CloudDark());

        yield return new WaitForSeconds(4f);

        deathScreen.SetActive(true);
    }

    IEnumerator CloudDark()
    {
        Debug.Log("FUCK");
        for(float i = 1; i > 0.3; i -= 0.05f)
        {
            yield return new WaitForSeconds(0.05f);
            cloudSprite.color = new Color(i, i, i, playerSprite.color.a);
        }
    }

}

