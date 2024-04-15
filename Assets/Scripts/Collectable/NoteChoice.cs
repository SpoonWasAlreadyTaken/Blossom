using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class NoteChoice : MonoBehaviour
{
    [Header("Quotes")]

    [SerializeField] private string[] goodQuotes;
    [SerializeField] private string[] badQuotes;
    [SerializeField] private TextMeshProUGUI quoteText;

    [Header("Inputs")]
    [SerializeField] private GameObject closedNote;
    [SerializeField] private GameObject openNote;
    [SerializeField] int regainHitPoints = 5;


    private int choice;
    private int goodQuoteRandom;
    private int badQuoteRandom;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        Time.timeScale = 0f;
        choice = Random.Range(0, 2);

        goodQuoteRandom = Random.Range(0, goodQuotes.Length);
        badQuoteRandom = Random.Range(0, badQuotes.Length);

        Debug.Log(choice);

        openNote.SetActive(false);

        if (choice == 0)
        {
            quoteText.text = goodQuotes[goodQuoteRandom];
        }
        else
        {
            quoteText.text = badQuotes[badQuoteRandom];
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void CloseNote()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void OpenNote()
    {
        closedNote.SetActive(false);
        openNote.SetActive(true);

        Time.timeScale = 1f;

        if (choice == 0)
        {
            playerHealth.RegainHealth(regainHitPoints);
        }
        else
        {
            playerHealth.TakeDamage(regainHitPoints);
        }

        StartCoroutine(CloseNoteTimer());
    }

    private IEnumerator CloseNoteTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
