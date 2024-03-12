using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("GameVolume", volume);
    }

    public void SetFullScreene(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}
