using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromMovement : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;
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
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);

        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
