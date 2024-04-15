using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollectable : MonoBehaviour
{
    [SerializeField] private GameObject summonNote;
    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && summonNote != null)
        {
            Instantiate(summonNote);
        }
    }
}
