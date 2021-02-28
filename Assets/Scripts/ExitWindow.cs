using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitWindow : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public Button exitButton;
    public GameObject exitWindowUI;
    public Button okButton;
    public Button noButton;
    
    private void Start()
    {
        Button btn = exitButton.GetComponent<Button>();
        btn.onClick.AddListener(OpenExitWindow);
        Button okBtn = okButton.GetComponent<Button>();
        okButton.onClick.AddListener(ExitGame);
        
        Button btnNo = noButton.GetComponent<Button>();
        btnNo.onClick.AddListener(Resume);
        
    }

    private void OpenExitWindow()
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

    private void Resume()
    {
        exitWindowUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    private void Pause()
    {
        exitWindowUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
