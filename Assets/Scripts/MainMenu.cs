using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    private bool isOptions = false;
    [SerializeField] private GameObject[] buttons;

    [SerializeField] private GameObject menuBoard;
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject[] gameIntro;

    private void Start()
    {
        optionsMenu.SetActive(false);

        for (int i = 0; i < gameIntro.Length; i++)
        {
            gameIntro[i].SetActive(false);
        }

        StartCoroutine(ShowLogo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOptions)
        {
            optionsMenu.SetActive(false);
            isOptions = false;
        }
    }


    public void PlayGame()

    {
        StartCoroutine(PlayGameStart());
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        isOptions = true;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void ReturnMenu()
    {
        optionsMenu.SetActive(false);
        isOptions = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
        }
    }


    private IEnumerator PlayGameStart()
    {
        menuBoard.SetActive(false);

        for (int i = 0; i < gameIntro.Length; i++)
        {
            for (int e = 0; e < gameIntro.Length; e++)
            {
                gameIntro[e].SetActive(false);
            }
            gameIntro[i].SetActive(true);
            yield return new WaitForSeconds(3f);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator ShowLogo()
    {
        menuBoard.SetActive(false);
        logo.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        menuBoard.SetActive(true);
        logo.SetActive(false);
    }
}
