using System;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private bool isPaused;

    [SerializeField] private GameObject pauseUI;
    
    private void Start() => this.isPaused = false;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        this.pauseUI.SetActive(true);
        this.isPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        this.pauseUI.SetActive(false);
        this.isPaused = false;
        Time.timeScale = 1f;
    }

    public void ExitGame() => Application.Quit();
}
