using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button Menu;

    private void Awake()
    {
        Menu.onClick.AddListener(ClickMenu);
    }

    private void ClickMenu() {
        SceneManager.LoadScene(0);
    }
    private void Start()
    {
        GameManager.Instance.OnGamePause += Instance_OnGamePause;
        GameManager.Instance.OnGameResume += Instance_OnGameResume;
        hide();
    }

    private void Instance_OnGameResume(object sender, System.EventArgs e)
    {
        hide();
    }

    private void Instance_OnGamePause(object sender, System.EventArgs e)
    {
        show();
    }

    private void show() {
    gameObject.SetActive(true);
    }

    private void hide() {
        gameObject.SetActive(false);
    }
}
