using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public Button pauseButton;
    public GameObject pauseMenuUI;
    public Button mainMenuButton;
    public Button resumeButton;
    public Button replayButton;
    public Button okButton;

    private void Start()
    {
        Button btn = pauseButton.GetComponent<Button>();
        btn.onClick.AddListener(OpenPauseMenu);
        if(okButton != null)
        {
            Button okBtn = okButton.GetComponent<Button>();
            okButton.onClick.AddListener(OpenPauseMenu);
        }
        
        if(mainMenuButton != null)
        {
            Button btnMenu = mainMenuButton.GetComponent<Button>();
            btnMenu.onClick.AddListener(goToMainMenu);
        }
        
        if(resumeButton != null)
        {
            Button btnResume = resumeButton.GetComponent<Button>();
            btnResume.onClick.AddListener(Resume);
        }
        
        if(replayButton != null)
        {
            Button btnReplay = replayButton.GetComponent<Button>();
            btnReplay.onClick.AddListener(Replay);
        }
        
    }

    private void OpenPauseMenu()
    {
        if(GameManager.Instance != null)
        {
            if (!GameManager.Instance.IsGameOver)
            {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        else
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    void goToMainMenu()
    {
        SceneManager.LoadScene(1);
        PauseMenu.gameIsPaused = false;
        Time.timeScale = 1f;
    }

    private void Replay()
    {
       // GameManager.Instance.ClearGoals();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        PauseMenu.gameIsPaused = false;
        Time.timeScale = 1f;
    }
}
