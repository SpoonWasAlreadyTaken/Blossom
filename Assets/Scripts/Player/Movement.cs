using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement Inputs, can be changed manually
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float airMobilityMultiplier = .8f; //changes the amount of Horizontal speed the player has while in the airs

    //Jump customization values
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private bool doubleJump = false; //makes coyote time and jump buffering more or less useless
    [SerializeField] private float coyoteTime = 0.2f; //time you can jump after being off the ground
    [SerializeField] private float jumpBuffer = 0.3f; //time that your jumps can be queued up

    //Dodging customization values
    [SerializeField] private float dodgeTime = .5f;
    [SerializeField] private float dodgeDistance = 5f;
    [SerializeField] private float dodgeCooldown = .8f;
    [SerializeField] private float staminaMax = 20;
    [SerializeField] private float staminaRegeneration = 1f;

    //Unity imputs
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;
    

    //Private values for movement controller. Don't touch
    private bool jumped = true;
    private bool isFacingRight = true;
    private float horizontalSpeed;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    public bool isDodging = false;
    private float dodgeCD = 0;
    private float stamina;
    private bool regenerateStamina = true;


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

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0 || !jumped && Input.GetButtonDown("Jump")) //makes the player jump
        {
            jumped = true;
            player.velocity = new Vector2(player.velocity.x, jumpHeight);

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && player.velocity.y > 0f) //makes it so that the player can stop a bit of their jump height if they let go of the jump key
        {
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * .7f);

            coyoteTimeCounter = 0f;
        }

        if (IsGrounded() && doubleJump) //allows the player to double jump if enable
        {
            jumped = false;
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

    //handles the players movement at a fixed time so it can't move faster or slower depending on the FPS
    void FixedUpdate()
    {
        if (IsGrounded() && !isDodging)
        {
            player.velocity = new Vector2(horizontalSpeed * movementSpeed, player.velocity.y);
        }
        else if (!isDodging)
        {
            player.velocity = new Vector2(horizontalSpeed * movementSpeed * airMobilityMultiplier, player.velocity.y);
        }
    }

    //determined if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
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
        regenerateStamina = false;
        playerSprite.color = Color.blue;
        if (isFacingRight)
        {
            player.velocity = new Vector2(-dodgeDistance * .5f, player.velocity.y) ;
        }
        else
        {
            player.velocity = new Vector2(dodgeDistance * .5f, player.velocity.y);
        }

        yield return new WaitForSeconds(dodgeTime * .7f);

        dodgeCD = dodgeCooldown * .5f;
        isDodging = false;
        playerSprite.color = Color.white;
        regenerateStamina = true;
    }

    private IEnumerator HorizontalDodge()
    {
        isDodging = true;
        regenerateStamina = false;
        playerSprite.color = Color.green;
        player.velocity = new Vector2(player.velocity.x * dodgeDistance * .5f, player.velocity.y);

        yield return new WaitForSeconds(dodgeTime);

        dodgeCD = dodgeCooldown;
        isDodging = false;
        playerSprite.color = Color.white;
        regenerateStamina = true;
    }


    //Animation controls, compares the players velocity to determine its movement.
    private void UpdateAnimation()

    {
        if (Mathf.Abs(player.velocity.x) > 0f)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (player.velocity.y > 0.2 && !IsGrounded())
        {
            anim.SetBool("Jumping", true);
            anim.SetBool("Falling", false);
        }
        else if (player.velocity.y < -0.2 && !IsGrounded())
        {
            anim.SetBool("Falling", true);
            anim.SetBool("Jumping", false);
        }
        else
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }
    }

    //Object visibility
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, .15f);
    }
}

