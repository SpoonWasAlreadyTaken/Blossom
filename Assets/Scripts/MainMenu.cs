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
    [SerializeField] private GameObject intro;
    [SerializeField] private GameObject backButton;

    private int tutorialPage;

    [SerializeField] private SceneData sceneData;

    private void Start()
    {
        optionsMenu.SetActive(false);

        for (int i = 0; i < gameIntro.Length; i++)
        {
            gameIntro[i].SetActive(false);
        }

        intro.SetActive(false);

        StartCoroutine(ShowLogo());

        SceneData.hasSavedData = false;
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
        menuBoard.SetActive(false);
        intro.SetActive(true);
        tutorialPage = 0;
        Tutorial();
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        menuBoard.SetActive(false);
        isOptions = true;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void ReturnMenu()
    {
        optionsMenu.SetActive(false);
        menuBoard.SetActive(true);
        isOptions = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
        }
    }


    public void ButtonForward()
    {
        tutorialPage++;
        if (tutorialPage >= gameIntro.Length)
        {
            intro.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Tutorial();
        }
    }

    public void ButtonBack()
    {
        tutorialPage--;
        Tutorial();
    }

    private void Tutorial()
    {
        if (tutorialPage == 0)
        {
            backButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(true);
        }

        for (int i = 0; i  < gameIntro.Length; i++) 
        {
            gameIntro[i].SetActive(false);
        }
        gameIntro[tutorialPage].SetActive(true);
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
