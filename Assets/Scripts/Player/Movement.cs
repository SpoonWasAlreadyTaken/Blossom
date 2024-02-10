using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement Imputs, can be changed manually
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float movementSpeed = 5f;

    //Jump customization values
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private bool doubleJump = false; //makes coyote time and jump buffering more or less useless
    [SerializeField] private float coyoteTime = 0.2f; //time you can jump after being off the ground
    [SerializeField] private float jumpBuffer = 0.3f; //time that your jumps can be queued up


    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;
    

    //Private values for movement controller. Don't touch
    private bool jumped = true;
    private bool isFacingRight = true;
    private float horizontalSpeed;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;


    private void Update()
    {
        horizontalSpeed = Input.GetAxisRaw("Horizontal");

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
            player.velocity = new Vector2(player.velocity.x, player.velocity.y - .03f);
        }


        UpdateAnimation();

        Flip(); 
    }

    //handles the players movement at a fixed time so it can't move faster or slower depending on the FPS
    void FixedUpdate()
    {
        player.velocity = new Vector2(horizontalSpeed * movementSpeed, player.velocity.y);
    }

    //determined if the player is on the ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }


    //flips the player in the direction its moveing
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

        if (player.velocity.y > 0)
        {
            anim.SetBool("Jumping", true);
            anim.SetBool("Falling", false);
        }
        else if (player.velocity.y < 0)
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
        Gizmos.DrawWireSphere(groundCheck.position, .2f);
    }
}

