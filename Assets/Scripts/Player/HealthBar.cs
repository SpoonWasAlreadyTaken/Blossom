using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth player;
    public Movement movement;
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Slider sliderStamina;

    void FixedUpdate()
    {
        float fillValueHealth = player.hitPoints / player.hitPointMaximum;
        sliderHealth.value = fillValueHealth;

        float fillValueStamina = movement.stamina / movement.staminaMax;
        sliderStamina.value = fillValueStamina;
    }
}
