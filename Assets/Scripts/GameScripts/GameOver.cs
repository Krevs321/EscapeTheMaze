using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;

    [Header("Main Menu Buttons")]
    public Button restartButton;
    public Button backMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        restartButton.onClick.AddListener(RestartGM);
        backMenuButton.onClick.AddListener(BackMenu);

    }

    public void BackMenu()
    {
        HideAll();
        SceneManager.LoadScene("1 GameStartScene");
    }

    public void RestartGM()
    {
        HideAll();
        SceneManager.LoadScene("2 MazeScene");
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);      
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
    }
}
