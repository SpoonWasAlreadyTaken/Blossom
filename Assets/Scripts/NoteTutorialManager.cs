using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTutorialManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        Destroy(gameObject);
        Time.timeScale = 1f;
    }
}
