using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private bool GamePause = false;
    public event Action OnGamePause;
    public event Action OnGameResume;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChefMovement.Instance.OnPause += Instance_OnPause;
        
    }

    private void Instance_OnPause()
    {
        TogglePause();
    }
    private void TogglePause()
    {
        GamePause = !GamePause;
        if (GamePause)
        {
            Time.timeScale = 0f;
            OnGamePause?.Invoke();
        }
        else {
        Time.timeScale = 1f;
            OnGameResume?.Invoke();
        }
    }
}
