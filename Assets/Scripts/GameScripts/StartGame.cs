using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class StartGame : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;
    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button optionButton;
    public Button quitButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGM);
        optionButton.onClick.AddListener(EnableOption);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
        
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void StartGM()
    {
        HideAll();
        SceneManager.LoadScene("2 MazeScene");
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);        
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
    }
    public void EnableOption()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }
}
