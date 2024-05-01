using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSoud : MonoBehaviour
{
    [SerializeField] private AudioSource jump;


    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump.Play();
        }
    }
}
