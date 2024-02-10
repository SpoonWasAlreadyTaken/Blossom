using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private string[] lines;
    [SerializeField] private string[] speaker;
    [SerializeField] private float textSpeed = .1f;
    [SerializeField] GameObject dialogueBox;
    //Background values
    [SerializeField] private float npcDialogueRange = 3f;
    [SerializeField] GameObject playerCharacter;


    //Script values don't touch.
    private float distance;
    private bool endOfDialogue = false;
    private bool dialogueStarted = false;
    

    private int index;


    private void Start()
    {
        npcName.text = string.Empty;
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, playerCharacter.transform.position);



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
    }
}

