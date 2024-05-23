using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TunnelLight : MonoBehaviour
{
    [Header("Light")]

    [SerializeField] private Light2D lightTunnel;
    [SerializeField] private float lightSize = 100;
    [SerializeField] private float escapeTime = 100;
    [SerializeField] private float escapeSize = 5;

    [Header("Variables")]

    [SerializeField] private bool escaped = false;
    [SerializeField] private float size;

    [Header("Player")]

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;

    private float timer;


    void Start()
    {
        size = lightSize;
        escaped = false;
    }

    void Update()
    {
        size -= Time.deltaTime / (escapeTime / 100);

        lightTunnel.pointLightOuterRadius = size;

        if (escaped)
        {
            lightTunnel.intensity += Time.deltaTime;
        }


        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < escapeSize && !escaped) 
        {
            StartCoroutine(EscapeSequence());
        }

        if (distance > size)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && playerHealth != null)
            {
                playerHealth.TakeDamageIgnoreIFrames(1);
                timer = .35f;
            }
        }
    }

    private IEnumerator EscapeSequence()
    {
        escaped = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Victory");
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, size);
        Gizmos.DrawWireSphere(transform.position, escapeSize);
    }
}
