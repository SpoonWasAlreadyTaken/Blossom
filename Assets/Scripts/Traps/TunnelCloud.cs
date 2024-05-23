using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TunnelCloud : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Light2D glow;

    private void Awake()
    {
        glow.intensity = 0.02f;
        sprite.color = new Color(.6f, .6f, .8f, 1f);
        glow.color = new Color(.6f, .6f, .8f, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        sprite.color = Color.white;
        glow.intensity = 1f;
        glow.color = Color.white;
    }
}
