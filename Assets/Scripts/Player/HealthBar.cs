using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;
    [SerializeField] private Movement movement;

    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Slider sliderStamina;
    [SerializeField] private TextMeshProUGUI noteCounter;



    void FixedUpdate()
    {
        float fillValueHealth = player.hitPoints / player.hitPointMaximum;
        sliderHealth.value = fillValueHealth;

        float fillValueStamina = movement.stamina / movement.staminaMax;
        sliderStamina.value = fillValueStamina;

        noteCounter.text = player.noteCount.ToString();
    }
}
