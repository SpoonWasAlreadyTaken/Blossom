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
    [SerializeField] private bool doubleJump = false;


    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Private values for movement controller. Don't touch
    private bool jumped = true;
    private bool isFacingRight = true;
    private float horizontalSpeed;


    private void Update()
    {
        horizontalSpeed = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown("w"))
        {
            Jump();
        }

        if (IsGrounded() && doubleJump)
        {
            jumped = false;
        }

        Flip();

    }

    void FixedUpdate()
    {
        player.velocity = new Vector2(horizontalSpeed * movementSpeed, player.velocity.y);
    }

   private void Jump()
    {
        if (IsGrounded() || !jumped)
        {
            jumped = true;
            player.velocity = new Vector2(player.velocity.x, jumpHeight);

            Debug.Log("Jumped");
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

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


    //Object visibility
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, .2f);
    }
}

