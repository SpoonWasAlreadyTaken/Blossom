using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement")]

    //Movement Inputs, can be changed manually
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float airMobilityMultiplier = .8f; //changes the amount of Horizontal speed the player has while in the airs
    [SerializeField] private bool enableSprinting = false;


    [Header("Jumping")]

    //Jump customization values
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private int extraJumps = 0; //makes coyote time and jump buffering more or less useless depending on jump count
    [SerializeField] private float coyoteTime = 0.2f; //time you can jump after being off the ground
    [SerializeField] private float jumpBuffer = 0.3f; //time that your jumps can be queued up

    [Header("Dodging")]

    //Dodging customization values
    [SerializeField] private float dodgeTime = .5f;
    [SerializeField] private float dodgeDistance = 5f;
    [SerializeField] private float dodgeCooldown = .8f;
    public float staminaMax = 20;
    [SerializeField] private float staminaRegeneration = 1f;

    [Header("Unity Inputs")]

    //Unity inputs
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckSize = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Animator animPlayer;
    [SerializeField] private Animator animCloud;
    [SerializeField] private SpriteRenderer playerSprite;


    //Private values for movement controller. Don't touch
    private int jumps = 0;
    private bool isFacingRight = true;
    private float horizontalSpeed;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    public bool isDodging = false;
    private float dodgeCD = 0;
    public float stamina;
    private bool regenerateStamina = true;
    private float sprintSpeed;
    private bool sprinting;
    private bool normalDodge = false;
    private bool neutralDodge = false;
    private float jumpBoost;


    private void Awake()
    {
        stamina = staminaMax; 
    }

    private void Update()
    {
        horizontalSpeed = Input.GetAxisRaw("Horizontal");

        //jumping functions

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBuffer;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0 || jumps < extraJumps && Input.GetButtonDown("Jump")) //makes the player jump
        {
            jumps += 1;
            player.velocity = new Vector2(player.velocity.x, jumpHeight * jumpBoost);

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && player.velocity.y > 0f) //makes it so that the player can stop a bit of their jump height if they let go of the jump key
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * .7f);

            coyoteTimeCounter = 0f;
        }

        if (IsGrounded()) //allows the player to double jump if enable
        {
            jumps = 0;
        }

        if (!IsGrounded() && player.velocity.y < 0)
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y - .04f);
        }

        //Dodging

        if (IsGrounded() && Mathf.Abs(player.velocity.x) > 0 && Input.GetKeyDown("space") && dodgeCD <= 0 && !isDodging && stamina > 0)
        {
            StartCoroutine(HorizontalDodge());

            stamina -= 5;
        }
        else if (IsGrounded() && Input.GetKeyDown("space") && dodgeCD <= 0 && !isDodging && stamina > 0)
        {
            StartCoroutine(NeutralDodge());

            stamina -= 3;
        }

        if (dodgeCD > 0)
        {
            dodgeCD -= Time.deltaTime; 
        }

        if (stamina <= staminaMax && regenerateStamina)
        {
            stamina += Time.deltaTime * staminaRegeneration;
        }

        //call functions
        UpdateAnimation();

        Flip(); 
    }

    //handles the players movement at a fixed time (20 times a second) so it can't move faster or slower depending on the FPS
    void FixedUpdate()
    {
        if (IsGrounded() && !isDodging)
        {
            player.velocity = new Vector2(horizontalSpeed * movementSpeed * sprintSpeed, player.velocity.y);
        }
        else if (!isDodging)
        {
            player.velocity = new Vector2(horizontalSpeed * movementSpeed * airMobilityMultiplier * sprintSpeed, player.velocity.y);
        }

        if (enableSprinting && Input.GetKey(KeyCode.LeftShift) && stamina > 5)
        {
            sprinting = true;
        }

        if (sprinting && !isDodging && stamina > 0 && Mathf.Abs(player.velocity.x) > 0f)
        {
            sprintSpeed = 1.5f;
            jumpBoost = 1.15f;
            animPlayer.SetFloat("WalkSpeed", 2f);
            stamina -= .1f;
            regenerateStamina = false;
            playerSprite.color = Color.red;
        }
        else if (!isDodging)
        {
            regenerateStamina = true;
            sprintSpeed = 1f;
            jumpBoost = 1f;
            animPlayer.SetFloat("WalkSpeed", 1f);
            playerSprite.color = Color.white;
            sprinting = false;
        }

        if (IsWalled() && isFacingRight)
        {
            player.velocity = new Vector2(-.01f, player.velocity.y);
        }
        else if (IsWalled())
        {
            player.velocity = new Vector2(.01f, player.velocity.y);
        }

        if (neutralDodge)
        {
            if (isFacingRight)
            {
                player.velocity = new Vector2(-dodgeDistance, player.velocity.y);
            }
            else
            {
                player.velocity = new Vector2(dodgeDistance, player.velocity.y);
            }
        }

        if (normalDodge)
        {
            if (isFacingRight)
            {
                player.velocity = new Vector2(player.velocity.x * .825f + dodgeDistance, player.velocity.y);
            }
            else
            {
                player.velocity = new Vector2(player.velocity.x * .825f - dodgeDistance, player.velocity.y);
            }
        }
    }


    //determined if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckSize, ground);
    }

    //flips the player in the direction its moving
    private void Flip()
    {
        if (isFacingRight && horizontalSpeed < 0f || !isFacingRight && horizontalSpeed > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator NeutralDodge()
    {
        isDodging = true;
        neutralDodge = true;
        regenerateStamina = false;
        playerSprite.color = Color.blue;


        yield return new WaitForSeconds(dodgeTime * .7f);

        dodgeCD = dodgeCooldown * .5f;
        isDodging = false;
        neutralDodge = false;
        playerSprite.color = Color.white;
        regenerateStamina = true;
    }

    private IEnumerator HorizontalDodge()
    {
        isDodging = true;
        normalDodge = true;
        regenerateStamina = false;
        playerSprite.color = Color.green;

        yield return new WaitForSeconds(dodgeTime);

        dodgeCD = dodgeCooldown;
        isDodging = false;
        normalDodge = false;
        playerSprite.color = Color.white;
        regenerateStamina = true;
    }


    //Animation controls, compares the players velocity to determine its movement.
    private void UpdateAnimation()

    {
        if (Mathf.Abs(player.velocity.x) > 0.1f)
        {
            animPlayer.SetBool("Walking", true);
        }
        else
        {
            animPlayer.SetBool("Walking", false);
        }

        if (player.velocity.y > 0.2 && !IsGrounded())
        {
            animPlayer.SetBool("Jumping", true);
            animPlayer.SetBool("Falling", false);
            animCloud.SetBool("Jumping", true);
            animCloud.SetBool("Falling", false);
        }
        else if (player.velocity.y < -0.2 && !IsGrounded())
        {
            animPlayer.SetBool("Falling", true);
            animPlayer.SetBool("Jumping", false);
            animCloud.SetBool("Falling", true);
            animCloud.SetBool("Jumping", false);
        }
        else
        {
            animPlayer.SetBool("Jumping", false);
            animPlayer.SetBool("Falling", false);
            animCloud.SetBool("Jumping", false);
            animCloud.SetBool("Falling", false);
        }

        if (normalDodge)
        {
            animPlayer.SetBool("Dodging", true);
        }
        else
        {
            animPlayer.SetBool("Dodging", false);
        }

        if (neutralDodge)
        {
            animPlayer.SetBool("nDodging", true);
        }
        else
        {
            animPlayer.SetBool("nDodging", false);
        }
    }

    //Object visibility
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, .15f);
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckSize);
    }
}

