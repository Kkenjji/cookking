using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button Play;
    [SerializeField] private Button Quit;

    private void Awake()
    {
        Play.onClick.AddListener(ClickPlay);
        Quit.onClick.AddListener(ClickQuit);
        Time.timeScale = 1f;
    }

    private void ClickPlay() {
        SceneManager.LoadScene(1);
    }
    private void ClickQuit() {
    Application.Quit();
        Debug.Log("quit");
    }
}
