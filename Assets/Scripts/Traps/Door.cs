using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : MonoBehaviour
{
    [SerializeField] private float doorSize = 3f;

    [SerializeField] GameObject playerCharacter;


    private void FixedUpdate()
    {

        float distance = Vector2.Distance(transform.position, playerCharacter.transform.position);

            if (Input.GetKeyDown(KeyCode.E) && distance < doorSize)
        {
            SceneManager.LoadScene("Door");
            Debug.Log("Door");
        }
    }

}
