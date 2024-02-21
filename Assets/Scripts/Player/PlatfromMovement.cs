using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromMovement : MonoBehaviour
{
    [SerializeField] private Collider2D headCollider;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private float disableCollider = 1f;

    //private variables for script. Don't touch
    private GameObject currentPlatform;

    void Update()
    {
        if (Input.GetKey("s"))
        {
            if (currentPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentPlatform.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(headCollider, platformCollider);
        Physics2D.IgnoreCollision(bodyCollider, platformCollider);

        yield return new WaitForSeconds(disableCollider);
        Physics2D.IgnoreCollision(headCollider, platformCollider, false);
        Physics2D.IgnoreCollision(bodyCollider, platformCollider, false);

    }
}
