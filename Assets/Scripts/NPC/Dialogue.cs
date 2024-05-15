
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private string[] lines;
    [SerializeField] private string[] speaker;
    [SerializeField] private float textSpeed = .1f;
    [SerializeField] private GameObject dialogueBox;

    //Background values
    [SerializeField] private float npcDialogueRange = 3f;


    [Header("Enemy Spawn")]

    [SerializeField] private bool spawnEnemyOnDialogueEnd = false;
    [SerializeField] private GameObject enemyToSpawn;

    [Header("Defeated Enemy")]

    [SerializeField] private bool proceedToNextArea = false;
    [SerializeField] private Transform doorOrigin;
    [SerializeField] private float doorSize = 3f;
    [SerializeField] private Vector3 doorLocation;


    //Script values don't touch.
    private float distance;
    private bool endOfDialogue = false;
    private bool dialogueStarted = false;
    private bool ableToProceed = false;
    private float distanceDoor;
    private GameObject playerCharacter;
    private PlayerHealth playerHealth;


    private int index;

    private void Awake()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        playerHealth = playerCharacter.GetComponent<PlayerHealth>();
    }



    void Update()
    {
        distance = Vector2.Distance(transform.position, playerCharacter.transform.position);

        if (doorOrigin != null ) 
        {
            distanceDoor = Vector2.Distance(doorOrigin.position, playerCharacter.transform.position);
        }


        if (Input.GetMouseButtonDown(0) && distance < npcDialogueRange && dialogueStarted)
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }

        if (Input.GetMouseButtonDown(0) && distance < npcDialogueRange && !dialogueStarted)
        {
            StartDialogue();
            dialogueStarted = true;
        }

        if (proceedToNextArea)
        {
            if (ableToProceed && distanceDoor < doorSize && Input.GetKeyDown(KeyCode.E))
            {
                playerHealth.NextLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        IsNearNPC();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        npcName.text = speaker[index];
    }

    IEnumerator TypeLine()
    {
        npcName.text = speaker[index];
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            npcName.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            endOfDialogue = true;

            if (spawnEnemyOnDialogueEnd && enemyToSpawn != null)
            {
                Instantiate(enemyToSpawn, transform.position, transform.rotation);
                Object.Destroy(gameObject);
            }

            if (proceedToNextArea && doorOrigin != null)
            {
                doorOrigin.position = Vector3.MoveTowards(this.transform.position, doorLocation, 10000);
                ableToProceed = true;
            }
        }
    }

    private void IsNearNPC()
    {
        float distance = Vector2.Distance(transform.position, playerCharacter.transform.position);

        if (distance < npcDialogueRange && !endOfDialogue)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }



    //Object visibility
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, npcDialogueRange);

        if (doorOrigin != null)
        {
            Gizmos.DrawWireSphere(doorOrigin.position, doorSize);
        }
    }
}

