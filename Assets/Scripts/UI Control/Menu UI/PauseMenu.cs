using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public GameObject pauseBackground;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject restartConfirmationUI;
    public GameObject exitConfirmationUI;
    public GameObject gameElementsUI;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsMenuUI.activeSelf && !restartConfirmationUI.activeSelf && !exitConfirmationUI.activeSelf)
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        FindObjectOfType<AudioManager>().PlaySFX("Pause");
        pauseMenuUI.SetActive(true);
        pauseBackground.SetActive(true);
        gameElementsUI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void FreezeGame()
    {
        pauseBackground.SetActive(true);
        gameElementsUI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        FindObjectOfType<AudioManager>().PlaySFX("Back");
        pauseMenuUI.SetActive(false);
        pauseBackground.SetActive(false);
        gameElementsUI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Reset()
    {
        pauseMenuUI.SetActive(false);
        pauseBackground.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void ExitToMenu()
    {
        FindObjectOfType<AudioManager>().PlaySFX("Select");
        SceneManager.LoadScene("MainMenu");
        Reset();
    }

    public void RestartLevel()
    {
        int currScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currScene);
    }

    public void NextLevel()
    {
        FindObjectOfType<LevelComplete>().Complete(1);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}
