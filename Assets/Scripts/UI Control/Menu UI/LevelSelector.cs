using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public Button[] buttons;
    public int levelsUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        UpdateStars();
    }

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene(levelNum);
    }

    public void Initialize()
    {
        levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < levelsUnlocked; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void UpdateStars()
    {
        for (int i = 0; i < levelObjects.Length; i++)
        {
            int starCount = PlayerPrefs.GetInt("Level " + (i + 1) + " StarCount", 0);
            levelObjects[i].ActivateStars(starCount);
        }
    }

    public void ResetProgress()
    {    
        PlayerPrefs.DeleteAll();
        Initialize();
        UpdateStars();
    }
}
