using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : MonoBehaviour
{
    private bool isInsideDoor = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInsideDoor = true;
        }
    }


    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E) && isInsideDoor)
        {
            SceneManager.LoadScene("Door");
            Debug.Log("Door");
        }
    }
}
