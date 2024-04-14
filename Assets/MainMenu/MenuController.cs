using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    string playScene = "LevelOne";
    [SerializeField]
    string mainMenuScene = "MainMenu";

    [Tooltip("Drag in a pause menu panel, if one exist")]
    [SerializeField]
    GameObject pauseMenuPanel;

    [SerializeField]
    bool IsPauseMenuAvailable = false;
    [HideInInspector]
    public static bool IsGamePaused = false;

    [Tooltip("Drag in a high scores panel, if one exists")]
    [SerializeField]
    GameObject HighScoresPanel;

    void Update()
    {
        PauseMenu();
    }

    public void PauseMenu()
    {
        if (IsPauseMenuAvailable)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsGamePaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void ReturnToMainMenu()
    {
        Resume();
        Cursor.visible = true;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void StartGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(playScene);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(playScene);
        Resume();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    //public void HighScoreMenuClose()
    //{
    //    HighScoresPanel.SetActive(false);
    //}

    //public void HighScoreMenuOpen()
    //{
    //    HighScoresPanel.GetComponent<HighScoreSystem>().UpdateHighScoreUI();
    //    HighScoresPanel.SetActive(true);
    //}
}