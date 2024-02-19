using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth player;
    public Image image;
    [SerializeField] private Slider slider;

    void FixedUpdate()
    {
        float fillValue = player.hitPoints / player.hitPointMaximum;
        slider.value = fillValue;
    }
}
